using Microsoft.AspNetCore.Mvc;

namespace AccessOperationTeam.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {

    }
}
