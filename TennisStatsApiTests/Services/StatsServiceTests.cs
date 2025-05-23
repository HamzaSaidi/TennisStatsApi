using Moq;
using TennisStatsApi.Models;
using TennisStatsApi.Repository;
using TennisStatsApi.Services;

namespace TennisStatsApiTests.Services;

public class StatsServiceTests
{
    private Mock<IPlayerRepository> _playerRepository;
    private IStatsService _statsService;

    public StatsServiceTests()
    {
        _playerRepository = new Mock<IPlayerRepository>();
        var testPlayers = new List<Player>
        {
            new Player {
                Data = new PlayerData { Height = 180, Weight = 75000, Last = new List<int>(){1, 1, 0}, Rank = 5 },
                Country = new Country { Code = "USA" }
            },
            new Player {
                Data = new PlayerData { Height = 190, Weight = 85000, Last = new List<int>(){1, 0, 0}, Rank = 10 },
                Country = new Country { Code = "FRA" }
            },
            new Player {
                Data = new PlayerData { Height = 170, Weight = 65000, Last = new List<int> {0, 0, 1}, Rank = 20 },
                Country = new Country { Code = "USA" }
            },
            new Player {
                Data = new PlayerData { Height = 200, Weight = 90000, Last = new List<int>() {1, 1, 1}, Rank = 1 },
                Country = new Country { Code = "ESP" }
            }
        };

        _playerRepository.Setup(repo => repo.GetAll()).ReturnsAsync(testPlayers.AsQueryable);

        _statsService = new StatsService(_playerRepository.Object);
        
    }
    
    
    
    [Fact]
    public async Task MedianePlayerHeight_ReturnsCorrectMedian()
    {
        var median =await  _statsService.MedianePlayerHeight();
        
        // Heights ordered: 170, 180, 190, 200
        // Count = 4, median index = 2 (0-based), element at index 2 is 190
        Assert.Equal(190, median);
    }

    [Fact]
    public async Task AverageBmi_ReturnsCorrectAverage()
    {
        var avgBmi = await _statsService.AverageBmi(); 

        Assert.InRange(avgBmi, 22.9, 23.0);
    }

    [Fact]
    public async Task CountryWithMaxWiningRatio_ReturnsCorrectCountryCode()
    {
        var countryCode = await _statsService.CountryWithMaxWiningRatio();
 
        Assert.Equal("ESP", countryCode);
    }

    [Fact]
    public async Task GetStats_ReturnsAllStatsCorrectly()
    {
        var stats = await _statsService.GetStats();

        Assert.Equal(190, stats.MedianPlayersHeight); // Check median height
        Assert.Equal("ESP", stats.CountryWithHighestWinRatio); // Check country
        // MedianPlayersHeight overwritten in your code with average BMI, likely a bug
        // Assuming you meant stats.AverageBmi
        Assert.InRange(stats.AveragePlayersBMI, 22.9, 23.0);
    }
}