using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using GenericRepository.Demo.Dtos.Request;
using GenericRepository.Demo.Entities;
using Repository.Generic;
using GenericRepository.Demo.Dtos.Filter;

namespace GenericRepository.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("authors/{authorId}")]
        public async Task<IActionResult> AddArticleAsync([FromRoute] Guid authorId, [FromBody] ArticleRequestDto dto)
        {
            var entity = await _unitOfWork.Repository.AddAsync<Article, Guid>(new Article
            {
                Topic = dto.Topic,
                Content = dto.Content,
                AuthorId = authorId
            }, default);

            await _unitOfWork.Repository.CompleteAsync();

            return Ok(entity);
        }

        [HttpPost("authors/{authorId}/range")]
        public async Task<IActionResult> AddArticleAsync([FromRoute] Guid authorId, [FromBody] List<ArticleRequestDto> dto)
        {
            var entity = await _unitOfWork.Repository.AddRangeAsync<Article, Guid>(dto.Select(s => new Article
            {
                Topic = s.Topic,
                Content = s.Content,
                AuthorId = authorId
            }).ToList(), default);

            await _unitOfWork.Repository.CompleteAsync();
            return Ok(entity);
        }

        [HttpPut("{id}/authors/{authorId}")]
        public async Task<IActionResult> UpdateArticleAsync([FromRoute] Guid id, [FromRoute] Guid authorId, [FromBody] ArticleRequestDto dto)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Article>(true, id);
            entity.Topic = dto.Topic;
            entity.Content = dto.Content;
            entity.AuthorId = authorId;
            var result = await _unitOfWork.Repository.UpdateAsync<Article, Guid>(entity);
            await _unitOfWork.Repository.CompleteAsync();
            return Ok(result);
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDeleteAsync([FromRoute] Guid id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Article>(true, id);
            await _unitOfWork.Repository.HardDeleteAsync<Article>(entity);
            await _unitOfWork.Repository.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteAsync([FromRoute] Guid id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Article>(true, id);
            await _unitOfWork.Repository.SoftDeleteAsync<Article, Guid>(entity);
            await _unitOfWork.Repository.CompleteAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var entity = await _unitOfWork.Repository.GetByIdAsync<Article>(true, id);
            return Ok(entity);
        }

        [HttpGet("multiple")]
        public async Task<IActionResult> GetMultipleAsync()
        {
            var entities = await _unitOfWork.Repository.GetMultipleAsync<Article>(false);
            return Ok(entities);
        }

        [HttpGet("selectable-multiple")]
        public async Task<IActionResult> GetSelectableMultipleAsync()
        {
            var entities = await _unitOfWork.Repository.GetMultipleAsync<Article, object>(false, select => new
            {
                SelectTopic = select.Topic,
                SelectContent = select.Content
            });
            return Ok(entities);
        }

        [HttpGet("filterable-multiple")]
        public async Task<IActionResult> GetFilterableMultipleAsync([FromQuery] ArticleFilterDto dto)
        {
            var entities = await _unitOfWork.Repository.GetMultipleAsync<Article, ArticleFilterDto, object>(false, dto, select => new
            {
                SelectTopic = select.Topic,
                SelectContent = select.Content
            });
            return Ok(entities);
        }

        [HttpGet("includable-filterable-selectable-multiple")]
        public async Task<IActionResult> GetIncludeableFilterableMultipleAsync([FromQuery] ArticleFilterDto dto)
        {
            Func<IQueryable<Article>, IIncludableQueryable<Article, object>> include = a => a.Include(i => i.Author);
            var entities = await _unitOfWork.Repository.GetMultipleAsync<Article, ArticleFilterDto, object>(false, dto, select => new
            {
                SelectName = select.Topic,
                SelectDate = select.CreationDate,
                Author = select.Author.Name
            }, include);
            return Ok(entities);
        }
    }
}

