using Microsoft.AspNetCore.Mvc;
using TennisStatsApi.Services;

namespace TennisStatsApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

     [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var players = await _playerService.GetAllOrderedByRank();
        return Ok(players); 
    }

     
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    { 
            var player = await _playerService.GetPlayeById(id);
            return Ok(player); 
    }
}