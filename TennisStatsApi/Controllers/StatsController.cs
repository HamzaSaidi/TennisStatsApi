using Microsoft.AspNetCore.Mvc;
using TennisStatsApi.Models;
using TennisStatsApi.Services;

namespace TennisStatsApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class StatsController:ControllerBase
{
    private IStatsService _statsService;

    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStats()
    {

        var stats =await _statsService.GetStats(); 
        return Ok(stats);

    }
}