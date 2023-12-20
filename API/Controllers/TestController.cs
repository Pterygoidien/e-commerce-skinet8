using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet(Name = "GetTest")]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
}
