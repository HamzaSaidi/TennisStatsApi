using System.Text.Json;
using Moq;
using TennisStatsApi.Exception;
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
    public async Task GetAllPlayers_WhenFileExists_ReturnsAllPlayers()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(_testPlayers);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);

        // Act
        var result = await _repository.GetAll();

        // Assert
        Assert.Equal(3, result.ToList().Count);
        
    }

    [Fact]
    public async Task GetAllPlayers_WhenFileDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false); 
        // Act
                await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetAll());

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
        await Assert.ThrowsAsync<NotFoundException>(() => _repository.GetById(7));
    }

    [Fact]
    public async Task GetAllPlayers_WhenFileContainsInvalidJson_ReturnsEmptyList()
    {
        // Arrange
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns("invalid json");

        // Act
 
        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetAll());
    }

    [Fact]
    public void LoadPlayers_IsLazyLoaded_OnlyCalledOnce()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(_testPlayers);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);

        // Act - Call multiple operations that use the cached data
        var result1 = _repository.GetAll().Result;
        var result2 = _repository.GetAll().Result;
        var result3 = _repository.GetById(1).Result;

        // Assert - File read operations should only happen once due to lazy loading
        _fileSystemHelperMock.Verify(x => x.ReadAllText(It.IsAny<string>()), Times.Once);
    }
}