using AutoMapper;
using DataAccess.DTOs;
using DataAccess.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Dtos;

namespace CarMSApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarModelController : ControllerBase
    {
        private readonly IGenericRepository<CarModel> _repo;
        private readonly IMapper _mapper;

        public CarModelController(IGenericRepository<CarModel> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ModelDto>>(items);
            return Ok(dtos);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var paged = await _repo.GetListPagedAsync(pageNumber, pageSize);
            var data = _mapper.Map<IEnumerable<ModelDto>>(paged.Data);
            var response = new PageableListResponseDTO<ModelDto>
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
            var dto = _mapper.Map<ModelDto>(item);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create(ModelDto dto)
        {
            var item = _mapper.Map<CarModel>(dto);
            _repo.Add(item);
            var created = _mapper.Map<ModelDto>(item);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ModelDto dto)
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
