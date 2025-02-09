using Microsoft.AspNetCore.Mvc;

namespace ECommerceSkinet.WebAPI.Controllers.v1
{
    // i want this to be with MVC Controller when the page opens i can go there and see the products
    [ApiController]
    [Route("api/{version:apiVersion}/ProductAdmin")]
    public class ProductAdminController : Controller
    {
        // This action method will handle the request and return a view with the products
        [HttpGet]
        public IActionResult Index()
        {
            // You can replace this with actual logic to fetch products from your database
            var products = new List<string> { "Product1", "Product2", "Product3" };
            return View(products);
        }
    }
}