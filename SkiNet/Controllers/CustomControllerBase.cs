using ECommerceSkinet.Core.Entities;
using ECommerceSkinet.Core.Helpers;
using ECommerceSkinet.Core.Interfaces;
using ECommerceSkinet.Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSkinet.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
        protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repo,
            ISpecification<T> spec, int pageIndex, int pageSize) where T : BaseEntity
        {
            var items = await repo.ListAsync(spec);
            var count = await repo.CountAsync(spec);

            var pagination = new Pagination<T>(pageIndex, pageSize, count, items);
            return Ok(pagination);
        }

        // New overload for DTOs
        protected ActionResult CreatePagedResult<TDto>(IReadOnlyList<TDto> data, int pageIndex, int pageSize, int totalItems)
            where TDto : class
        {
            var pagination = new Pagination<TDto>(pageIndex, pageSize, totalItems, data);
            return Ok(pagination);
        }
    }
}
