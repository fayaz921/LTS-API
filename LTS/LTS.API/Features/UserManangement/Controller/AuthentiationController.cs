using Microsoft.AspNetCore.Mvc;

namespace LTS.API.Features.UserManangement.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentiationController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Authentication Endpoint");
        }
    }
}
