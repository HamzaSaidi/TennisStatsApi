using Microsoft.AspNetCore.Mvc;
using Moq;
using TennisStatsApi.Controllers;
using TennisStatsApi.Exception;
using TennisStatsApi.Models;
using TennisStatsApi.Services;

namespace TennisStatsApiTests.Controllers;

public class PlayersControllerTests
{
    private readonly Mock<IPlayerService> _mockService;
    private readonly PlayersController _controller;

    public PlayersControllerTests()
    {
        _mockService = new Mock<IPlayerService>();
        _controller = new PlayersController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfPlayers()
    {
        // Arrange
        var players = new List<Player>
        {
            new Player { Id = 1, Firstname = "Novak" },
            new Player { Id = 2, Firstname = "Rafael" }
        };
        _mockService.Setup(s => s.GetAllOrderedByRank()).ReturnsAsync(players);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPlayers = Assert.IsType<List<Player>>(okResult.Value);
        Assert.Equal(2, returnPlayers.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WithPlayer()
    {
        // Arrange
        var player = new Player { Id = 1, Firstname = "Novak" };
        _mockService.Setup(s => s.GetPlayeById(1)).ReturnsAsync(player);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPlayer = Assert.IsType<Player>(okResult.Value);
        Assert.Equal("Novak", returnPlayer.Firstname);
    }

    [Fact]
    public async Task GetById_ThrowsNotFoundException_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetPlayeById(It.IsAny<int>())).ThrowsAsync(new NotFoundException("Player not found"));

        // Act

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetById(99));
    }
}