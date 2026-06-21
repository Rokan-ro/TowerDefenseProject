using Microsoft.AspNetCore.Mvc;

namespace TowerDefense.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(new
        {
            success = true,
            message = "Tower Defense API is running.",
            data = new
            {
                status = "Healthy",
                framework = ".NET 10"
            }
        });
    }
}