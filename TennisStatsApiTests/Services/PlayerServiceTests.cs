using Moq;
using TennisStatsApi.Exception;
using TennisStatsApi.Models;
using TennisStatsApi.Repository;
using TennisStatsApi.Services;

namespace TennisStatsApiTests.Services;

public class PlayerServiceTests
{
    private readonly Mock<IPlayerRepository> _mockRepo;
    private readonly PlayerService _service;

    public PlayerServiceTests()
    {
        _mockRepo = new Mock<IPlayerRepository>();
        _service = new PlayerService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetPlayeById_ReturnsPlayer_WhenExists()
    {
        // Arrange
        var expectedPlayer = new Player
        {
            Id = 1,
            Firstname = "Novak",
            Data = new PlayerData { Rank = 2 }
        };

        _mockRepo.Setup(r => r.GetById(1)).ReturnsAsync(expectedPlayer);

        // Act
        var result = await _service.GetPlayeById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Novak", result.Firstname);
    }

    [Fact]
    public async Task GetPlayeById_ThrowsNotFoundException_WhenNotFound()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync((Player)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetPlayeById(99));
    }

    [Fact]
    public async Task GetAllOrderedByRank_ReturnsSortedPlayers()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player { Id = 1, Firstname = "Player A", Data = new PlayerData { Rank = 10 } },
            new Player { Id = 2, Firstname = "Player B", Data = new PlayerData { Rank = 5 } },
            new Player { Id = 3, Firstname = "Player C", Data = new PlayerData { Rank = 15 } }
        };

        _mockRepo.Setup(r => r.GetAll()).ReturnsAsync(players.AsQueryable);

        // Act
        var result = await _service.GetAllOrderedByRank();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Player C", result[0].Firstname); // Highest rank (15)
        Assert.Equal("Player A", result[1].Firstname); // Next highest (10)
        Assert.Equal("Player B", result[2].Firstname); // Lowest (5)
    }
}