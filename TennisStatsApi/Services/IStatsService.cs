using TennisStatsApi.Models;

namespace TennisStatsApi.Services;

public interface IStatsService
{
   public Task<Stats> GetStats();
   public Task<float> MedianePlayerHeight();

   public   Task<float> AverageBmi();

   public   Task<string> CountryWithMaxWiningRatio();



}