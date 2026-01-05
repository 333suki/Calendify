using Backend.Authorization;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("streak")]
public class StreakController : ControllerBase {
    private readonly IStreakService _streakService;
    
    public StreakController(IStreakService streakService) {
        _streakService = streakService;
    }
    
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpGet("")]
    public IActionResult GetOwnStreakCount()
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        Streak? streak = _streakService.GetStreak(Convert.ToInt32(payload!.Sub));

        if (streak is null) {
            return Ok(
                new {
                    count = 0
                }
            );
        }
        
        return Ok(
            new {
                count = streak.Count
            }
        );
    }
    
    [ServiceFilter(typeof(JwtAuthFilter))]
    [HttpPost("increase")]
    public IActionResult IncreaseOwnStreak()
    {
        var payload = HttpContext.Items["jwtPayload"] as Payload;
        
        _streakService.IncreaseStreak(Convert.ToInt32(payload!.Sub));
        return Ok();
    }
}
