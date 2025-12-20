
using AllulExpressClientApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/driver")]
[Authorize]
public class DriverLocationController : ControllerBase
{
    private readonly AppDbContext _db;

    public DriverLocationController(AppDbContext db)
    {
        _db = db;
    }
    [HttpGet("location/{id}")]
    public async Task<IActionResult> GetDriverById(int id)
    {
        var driver = await _db.Drivers
            .Where(d => d.Id == id)
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.Latitude,
                d.Longitude
            })
            .FirstOrDefaultAsync();

        if (driver == null) return NotFound();

        return Ok(driver);
    }


    [HttpPost("location")]
    public async Task<IActionResult> UpdateLocation(
        [FromBody] AllulExpressClientApi.Models.DriverLocationDto dto)
    {
        try
        {
            var driver = await _db.Drivers.FindAsync(dto.Id);
            if (driver == null)
                return NotFound("Driver not found");

            driver.Latitude = dto.Latitude;
            driver.Longitude = dto.Longitude;
            driver.LastUpdated = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok("Location updated");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
