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
    public class CarController : ControllerBase
    {
        private readonly IGenericRepository<Car> _repo;
        private readonly IGenericRepository<CarModel> _modelRepo;
        private readonly IMapper _mapper;

        public CarController(
            IGenericRepository<Car> repo,
            IGenericRepository<CarModel> modelRepo,
            IMapper mapper)
        {
            _repo = repo;
            _modelRepo = modelRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CarDTO>>(items);
            return Ok(dtos);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var paged = await _repo.GetListPagedAsync(pageNumber, pageSize);
            var data = _mapper.Map<IEnumerable<CarDTO>>(paged.Data);
            var response = new PageableListResponseDTO<CarDTO>
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
            var dto = _mapper.Map<CarDTO>(item);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create(CarDTO dto)
        {
            var model = _modelRepo.Get(dto.CarModelId);
            if (model == null) return BadRequest();

            var item = _mapper.Map<Car>(dto);
            _repo.Add(item);
            var created = _mapper.Map<CarDTO>(item);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CarDTO dto)
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
            _repo.Delete(id);
            return NoContent();
        }
    }
}
