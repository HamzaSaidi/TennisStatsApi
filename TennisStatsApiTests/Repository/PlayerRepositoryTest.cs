using System.Text.Json;
using Moq;
using TennisStatsApi.Helpers;
using TennisStatsApi.Models;
using TennisStatsApi.Repository;

namespace TennisStatsApiTests.Repository;

public class PlayerRepositoryTest
{
    private const string TestFilePath = "Data/test_players.json";
    private readonly PlayerRepository _repository;
    
    
    private readonly Mock<IFileSystemHelper> _fileSystemHelperMock;
     
    private readonly List<Player> _testPlayers = new List<Player>
    {
        new Player { Id = 1, Data = new PlayerData { Rank = 100 } },
        new Player { Id = 2, Data = new PlayerData { Rank = 200 } },
        new Player { Id = 3, Data = new PlayerData { Rank = 50 } }
    };

    public PlayerRepositoryTest()
    {
        _fileSystemHelperMock = new Mock<IFileSystemHelper>();
        _repository = new PlayerRepository(_fileSystemHelperMock.Object);
    }

    [Fact]
    public async Task GetAllPlayers_WhenFileExists_ReturnsPlayersOrderedByRankDesc()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(_testPlayers);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);

        // Act
        var result = await _repository.GetAllOrderedByRank();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(2, result[0].Id); // Highest rank (200) should be first
        Assert.Equal(1, result[1].Id); // Next highest (100)
        Assert.Equal(3, result[2].Id); // Lowest rank (50)
    }

    [Fact]
    public async Task GetAllPlayers_WhenFileDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false); 
        // Act
        var result = await _repository.GetAllOrderedByRank();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPlayerById_WhenPlayerExists_ReturnsCorrectPlayer()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(_testPlayers);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);

        // Act
        var result = await _repository.GetById(2);

        // Assert
        Assert.Equal(2, result.Id);
        Assert.Equal(200, result.Data.Rank);
    }

    [Fact]
    public async Task GetPlayerById_WhenPlayerDoesNotExist_ThrowsException()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(_testPlayers);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetById(99));
    }

    [Fact]
    public async Task GetAllPlayers_WhenFileContainsInvalidJson_ReturnsEmptyList()
    {
        // Arrange
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns("invalid json");

        // Act
        var result = await _repository.GetAllOrderedByRank();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void LoadPlayers_IsLazyLoaded_OnlyCalledOnce()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(_testPlayers);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);

        // Act - Call multiple operations that use the cached data
        var result1 = _repository.GetAllOrderedByRank().Result;
        var result2 = _repository.GetAllOrderedByRank().Result;
        var result3 = _repository.GetById(1).Result;

        // Assert - File read operations should only happen once due to lazy loading
        _fileSystemHelperMock.Verify(x => x.ReadAllText(It.IsAny<string>()), Times.Once);
    }
}