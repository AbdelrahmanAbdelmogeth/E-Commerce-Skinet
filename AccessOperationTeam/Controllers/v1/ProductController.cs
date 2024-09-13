using AccessOperationTeam.Infrastructure.DatabaseContext;
using AccessOperationTeam.WebAPI.Controllers;
using ECommerceSkinet.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSkinet.WebAPI.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class ProductController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct() {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
        
       
    }
}
