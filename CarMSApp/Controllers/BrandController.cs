using AutoMapper;
using DataAccess.DTOs;
using DataAccess.Models;
using DataAccess.Paging;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace CarMSApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IGenericRepository<Brand> _repo;
        private readonly IMapper _mapper;

        public BrandController(IGenericRepository<Brand> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<BrandDTO>>(items);
            return Ok(dtos);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var paged = await _repo.GetListPagedAsync(pageNumber, pageSize);
            var data = _mapper.Map<IEnumerable<BrandDTO>>(paged.Data);
            var response = new PageableListResponseDTO<BrandDTO>
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
            var dto = _mapper.Map<BrandDTO>(item);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Create(BrandDTO dto)
        {
            var item = _mapper.Map<Brand>(dto);
            _repo.Add(item);
            var created = _mapper.Map<BrandDTO>(item);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, BrandDTO dto)
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
