using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcessionariaAPP.Application.Api;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class VehicleApiController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new[] {
        new { Id = 1, Model = "Sedan X", Price = 95000m },
        new { Id = 2, Model = "SUV Y", Price = 125000m }
    });
}