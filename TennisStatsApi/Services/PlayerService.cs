using TennisStatsApi.Exception;
using TennisStatsApi.Models;
using TennisStatsApi.Repository;

namespace TennisStatsApi.Services;

public class PlayerService:IPlayerService
{
    private IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Player> GetPlayeById(int Id)
    {

       var player= await _playerRepository.GetById(Id);
        if(player==null)
            throw new NotFoundException("Player not found");
        return player;
    }

    public async Task<List<Player>> GetAllOrderedByRank()
    {
        var list = await _playerRepository.GetAll();
        return list.OrderByDescending(p=>p.Data.Rank).ToList();
    }
}