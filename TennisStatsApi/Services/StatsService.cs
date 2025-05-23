using TennisStatsApi.Models;
using TennisStatsApi.Repository;
using TennisStatsApi.Helpers;

namespace TennisStatsApi.Services;

public class StatsService:IStatsService
{
    private IPlayerRepository _playerRepository;

    public StatsService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    } 
    public async Task<Stats> GetStats()
    {
        var stats=new Stats();
        var medianHeightTask = MedianePlayerHeight();
        var countryWithMaxWiningTask = CountryWithMaxWiningRatio();
        var averageBmrTask = AverageBmi();
       
        await Task.WhenAll(medianHeightTask, countryWithMaxWiningTask, averageBmrTask);
       
        stats.MedianPlayersHeight=medianHeightTask.Result;
        stats.CountryWithHighestWinRatio=countryWithMaxWiningTask.Result;
        stats.AveragePlayersBMI=averageBmrTask.Result;
      
        return stats;

    }

    public async Task<float> MedianePlayerHeight()
    {
        var players= await _playerRepository.GetAll();
        var medianeIndex = players.Count() % 2 == 0 ?
                            players.Count() / 2 
                            : (1 + players.Count())/ 2; 
        
        return players.OrderBy(p => p.Data.Height).ElementAt(medianeIndex).Data.Height;
    }
    public async Task<float> AverageBmi()
    {
        var players= await _playerRepository.GetAll(); 
        
        Func<Player, float> getBmi = ((p) =>
        
        { var height = p.Data.Height/100 <1 ? p.Data.Height: p.Data.Height.ToMeters();
        var weight = p.Data.Weight/1000>1? p.Data.Weight.ToKilograms():p.Data.Weight;

        return weight / (height * height);

        }); 
       
        
        return players.Average(getBmi);
    }
    public async Task<string> CountryWithMaxWiningRatio()
    {
        var players= await _playerRepository.GetAll(); 
        return players.MaxBy(p => p.Data.Last.Sum())?.Country.Code;
         
    }
    
}