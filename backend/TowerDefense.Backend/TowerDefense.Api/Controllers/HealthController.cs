using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TowerDefense.Api.Data;

namespace TowerDefense.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly TowerDefenseDbContext _dbContext;

    public HealthController(TowerDefenseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        bool databaseConnected =
            await _dbContext.Database.CanConnectAsync();

        if (!databaseConnected)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    success = false,
                    message = "Tower Defense API is running, but the database is unavailable.",
                    data = new
                    {
                        apiStatus = "Healthy",
                        databaseStatus = "Unavailable",
                        framework = ".NET 10"
                    }
                }
            );
        }

        return Ok(new
        {
            success = true,
            message = "Tower Defense API and database are running.",
            data = new
            {
                apiStatus = "Healthy",
                databaseStatus = "Connected",
                framework = ".NET 10",
                database = "MySQL 8.4"
            }
        });
    }
}