using TennisStatsApi.Models;

namespace TennisStatsApi.Repository;

public   interface IPlayerRepository
{
   public Task<List<Player>> GetAllOrderedByRank();
   public  Task<Player> GetById(int id);
}