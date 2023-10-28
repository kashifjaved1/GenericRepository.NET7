using Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repository.Generic;
using GenericRepository.Demo.Dtos.Request;
using GenericRepository.Demo.Entities;
using GenericRepository.Demo.Specs;
using Repository.Ardalis.Specifications;
using GenericRepository.Demo.Dtos.Filter;

namespace GenericRepository.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorController(IRepository repository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "FilterAuthor")]
        public async Task<IActionResult> FilterAuthorAsync([FromQuery] string name)
        {
            var queryable = _unitOfWork.Repository.GetQueryable<Author>();
            //var spec = new AuthorByNameSpec(name);
            var spec = new AuthorOrderByNameSpec(name);
            var data = SpecificationConverter.Convert(queryable, spec);
            return Ok(data.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthorAsync([FromBody] AuthorRequestDto dto)
        {
            var entity = await _unitOfWork.Repository.AddAsync<Author, Guid>(new Author
            {
                Name = dto.Name,
                Surname = dto.Surname
            });

            await _unitOfWork.Repository.CompleteAsync();
            return Ok(entity);
        }

        [HttpPost("range")]
        public async Task<IActionResult> AddAuthorAsync([FromBody] List<AuthorRequestDto> dto)
        {
            var entity = await _unitOfWork.Repository.AddRangeAsync<Author, Guid>(dto.Select(s => new Author
            {
                Name = s.Name,
                Surname = s.Surname
            }).ToList(), default);

            return Ok(entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthorAsync([FromRoute] Guid id, [FromBody] AuthorRequestDto dto)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Author>(true, id);
            entity.Name = dto.Name;
            entity.Surname = dto.Surname;
            var result = await _unitOfWork.Repository.UpdateAsync<Author, Guid>(entity);
            return Ok(result);
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDeleteAsync([FromRoute] Guid id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Author>(true, id);
            await _unitOfWork.Repository.HardDeleteAsync<Author>(entity);
            return NoContent();
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteAsync([FromRoute] Guid id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Author>(true, id);
            await _unitOfWork.Repository.SoftDeleteAsync<Author, Guid>(entity);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Author>(true, id);
            return Ok(entity);
        }

        [HttpGet("multiple")]
        public async Task<IActionResult> GetMultipleAsync()
        {
            var entities = await _unitOfWork.Repository.GetMultipleAsync<Author>(false);
            return Ok(entities);
        }

        [HttpGet("selectable-multiple")]
        public async Task<IActionResult> GetSelectableMultipleAsync()
        {
            var entities = await _unitOfWork.Repository.GetMultipleAsync<Author, object>(false, select => new
            {
                SelectName = select.Name,
                SelectDate = select.CreationDate
            });
            return Ok(entities);
        }

        [HttpGet("filterable-multiple")]
        public async Task<IActionResult> GetFilterableMultipleAsync([FromQuery] AuthorFilterDto dto)
        {
            var entities = await _unitOfWork.Repository.GetMultipleAsync<Author, AuthorFilterDto, object>(false, dto, select => new
            {
                SelectName = select.Name,
                SelectDate = select.CreationDate
            });
            return Ok(entities);
        }

        [HttpGet("includable-filterable-selectable-multiple")]
        public async Task<IActionResult> GetIncludeableFilterableMultipleAsync([FromQuery] AuthorFilterDto dto)
        {
            Func<IQueryable<Author>, IIncludableQueryable<Author, object>> include = a => a.Include(i => i.Books)
               .Include(j => j.Articles);

            var entities = await _unitOfWork.Repository.GetMultipleAsync<Author, AuthorFilterDto, object>(false, dto, select => new
            {
                SelectName = select.Name,
                SelectDate = select.CreationDate,
                Books = select.Books.ToList(),
                Articles = select.Articles.ToList(),
            }, include);

            return Ok(entities);
        }
    }
}
