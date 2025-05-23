using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
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
     
    private readonly Root   PlayersRootTest
 = new Root(){
   Players = new List<Player>
{
    new Player
    {
        Id = 1,
        Firstname = "Novak",
        Lastname = "Djokovic",
        Shortname = "N.DJO",
        Sex = "M",
        Country = new Country
        {
            Code = "SRB",
            Picture = "https://tenisu.latelier.co/resources/Serbie.png"
        },
        Picture = "https://tenisu.latelier.co/resources/Djokovic.png",
        Data = new PlayerData
        {
            Rank = 2,
            Points = 2542,
            Weight = 80000,
            Height = 188,
            Age = 31,
            Last = new List<int> { 1, 1, 1, 1, 1 }
        }
    },
    new Player
    {
        Id = 2,
        Firstname = "Serena",
        Lastname = "Williams",
        Shortname = "S.WIL",
        Sex = "F",
        Country = new Country
        {
            Code = "USA",
            Picture = "https://tenisu.latelier.co/resources/USA.png"
        },
        Picture = "https://tenisu.latelier.co/resources/Serena.png",
        Data = new PlayerData
        {
            Rank = 10,
            Points = 3521,
            Weight = 72000,
            Height = 175,
            Age = 37,
            Last = new List<int> { 0, 1, 1, 1, 0 }
        }
    },
    new Player
    {
        Id = 3,
        Firstname = "Rafael",
        Lastname = "Nadal",
        Shortname = "R.NAD",
        Sex = "M",
        Country = new Country
        {
            Code = "ESP",
            Picture = "https://tenisu.latelier.co/resources/Espagne.png"
        },
        Picture = "https://tenisu.latelier.co/resources/Nadal.png",
        Data = new PlayerData
        {
            Rank = 1,
            Points = 1982,
            Weight = 85000,
            Height = 185,
            Age = 33,
            Last = new List<int> { 1, 0, 0, 0, 1 }
        }
    },
    new Player
    {
        Id = 4,
        Firstname = "Stan",
        Lastname = "Wawrinka",
        Shortname = "S.WAW",
        Sex = "M",
        Country = new Country
        {
            Code = "SUI",
            Picture = "https://tenisu.latelier.co/resources/Suisse.png"
        },
        Picture = "https://tenisu.latelier.co/resources/Wawrinka.png",
        Data = new PlayerData
        {
            Rank = 21,
            Points = 1784,
            Weight = 81000,
            Height = 183,
            Age = 33,
            Last = new List<int> { 1, 1, 1, 0, 1 }
        }
    }
}
    };

    private readonly Mock<IWebHostEnvironment> _env;

    public PlayerRepositoryTest()
    {
        _fileSystemHelperMock = new Mock<IFileSystemHelper>();
        _env = new Mock<IWebHostEnvironment>();
        _repository = new PlayerRepository(_fileSystemHelperMock.Object,_env.Object);
    }

    [Fact]
    public async Task GetAllPlayers_WhenFileExists_ReturnsAllPlayers()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(PlayersRootTest);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);
        _env.Setup(x => x.ContentRootPath).Returns("/root");
        // Act
        var result = await _repository.GetAll();

        // Assert
        Assert.Equal(4, result.ToList().Count);
        
    }


    [Fact]
    public async Task GetPlayerById_WhenPlayerExists_ReturnsCorrectPlayer()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(PlayersRootTest
);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);
        _env.Setup(x => x.ContentRootPath).Returns("/root");

        // Act
        var result = await _repository.GetById(2);

        // Assert
        Assert.Equal(2, result.Id);
        Assert.Equal(10, result.Data.Rank);
    }

    [Fact]
    public async Task GetPlayerById_WhenPlayerDoesNotExist_ThrowsException()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(PlayersRootTest
);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData);
        _env.Setup(x => x.ContentRootPath).Returns("/root");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _repository.GetById(7));
    }

  

    [Fact]
    public void LoadPlayers_IsLazyLoaded_OnlyCalledOnce()
    {
        // Arrange
        var jsonData = JsonSerializer.Serialize(PlayersRootTest
);
        _fileSystemHelperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
        _fileSystemHelperMock.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(jsonData); 
        _env.Setup(x => x.ContentRootPath).Returns("/root");
        // Act - Call multiple operations that use the cached data
        var result1 = _repository.GetAll().Result;
        var result2 = _repository.GetAll().Result;
        var result3 = _repository.GetById(1).Result;

        // Assert - File read operations should only happen once due to lazy loading
        _fileSystemHelperMock.Verify(x => x.ReadAllText(It.IsAny<string>()), Times.Once);
    }
}