using TennisStatsApi.Models;

namespace TennisStatsApi.Repository;

public   interface IPlayerRepository
{
   public Task<IQueryable<Player>> GetAll();
   public  Task<Player> GetById(int id);
}