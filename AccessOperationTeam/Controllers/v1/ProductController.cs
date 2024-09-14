using AccessOperationTeam.Infrastructure.DatabaseContext;
using AccessOperationTeam.WebAPI.Controllers;
using ECommerceSkinet.Core.Entities;
using ECommerceSkinet.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSkinet.WebAPI.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class ProductController : CustomControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductController(IProductRepository repo) {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts() {
            var products = await _repo.GetProductsAsync();
            if (products != null && products.Count != 0)
                return Ok(products);
            else
                return BadRequest("No Products Available");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Product>>> GetProduct(int id) {

            var product = await _repo.GetProductByIdAsync(id);
            if (product != null)
                return Ok(product);
            else
                return BadRequest("No product available with the given id");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repo.GetProductBrandsAsync());
        }


        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _repo.GetProductTypesAsync());
        }

    }
}
