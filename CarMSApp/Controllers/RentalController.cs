using AutoMapper;
using DataAccess.DTOs;
using DataAccess.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarMSApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IGenericRepository<Rental> _repo;
        private readonly IGenericRepository<Car> _carRepo;
        private readonly IGenericRepository<Brand> _brandRepo;
        private readonly IGenericRepository<CarModel> _carModelRepo;
        private readonly IMapper _mapper;

        public RentalController(
            IGenericRepository<Rental> repo,
            IGenericRepository<Car> carRepo,
            IMapper mapper,
            IGenericRepository<CarModel> carModelRepo,
            IGenericRepository<Brand> brandRepo)
        {
            _repo = repo;
            _carRepo = carRepo;
            _mapper = mapper;
            _carModelRepo = carModelRepo;
            _brandRepo = brandRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<RentalDTO>>(items);
            return Ok(dtos);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var paged = await _repo.GetListPagedAsync(pageNumber, pageSize);
            var data = _mapper.Map<IEnumerable<RentalDTO>>(paged.Data);
            var response = new PageableListResponseDTO<RentalDTO>
            {
                Data = data.ToList(),
                TotalCount = paged.TotalCount,
                CurrentPage = paged.CurrentPage,
                TotalPages = paged.TotalPages
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.Get(id);
            if (item == null) return NotFound();
            var dto = _mapper.Map<RentalDTO>(item);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create(RentalDTO dto)
        {
            var car = _carRepo.Get(dto.CarId);
            if (car == null || !car.IsAvailable) return BadRequest();

            car.IsAvailable = false;
            car.AvailableFrom = dto.EndDate;
            _carRepo.Update(car);

            car.CarModel = _carModelRepo.Get(car.CarModelId);
            car.CarModel.Brand = _brandRepo.Get(car.CarModel.BrandId);
            var rental = _mapper.Map<Rental>(dto);
            rental.Status = RentStatus.Rented;
            _repo.Add(rental);

            var created = _mapper.Map<RentalDTO>(rental);
            return Ok(created);
        }

        [HttpPut("complete/{id}")]
        public IActionResult CompleteRental(int id)
        {
            var rental = _repo.Get(id);
            if (rental == null) return NotFound();
            if (rental.Status == RentStatus.Completed) return BadRequest();

            rental.Status = RentStatus.Completed;
            _repo.Update(rental);

            var car = _carRepo.Get(rental.CarId);
            if (car != null)
            {
                car.IsAvailable = true;
                car.AvailableFrom = null;
                _carRepo.Update(car);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, RentalDTO dto)
        {
            var item = _repo.Get(id);
            if (item == null) return NotFound();
            _mapper.Map(dto, item);
            _repo.Update(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _repo.Get(id);
            if (item == null) return NotFound();

            if (item.Status == RentStatus.Rented)
            {
                var car = _carRepo.Get(item.CarId);
                if (car != null)
                {
                    car.IsAvailable = true;
                    car.AvailableFrom = null;
                    _carRepo.Update(car);
                }
            }

            _repo.Delete(id);
            return NoContent();
        }
    }
}
