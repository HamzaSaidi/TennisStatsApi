using TennisStatsApi.Models;

namespace TennisStatsApi.Services;

public interface IPlayerService
{
    public Task<Player> GetPlayeById(int Id);
    public Task<List<Player>> GetAllOrderedByRank();
    
}