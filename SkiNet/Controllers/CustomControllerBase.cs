using Microsoft.AspNetCore.Mvc;

namespace ECommerceSkinet.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {

    }
}
