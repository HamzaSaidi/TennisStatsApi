using TennisStatsApi.Models;

namespace TennisStatsApi.Repository;

public   interface IPlayerRepository
{
   public Task<List<Player>> GetAll();
   public  Task<Player> GetById(int id);
}