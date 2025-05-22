using System.Text.Json;
using TennisStatsApi.Helpers;
using TennisStatsApi.Models;

namespace TennisStatsApi.Repository;

public class PlayerRepository:IPlayerRepository
{
    private const string FilePath = "Data/data.json";
    private Lazy<List<Player>> _cachedPlayers;
     

    private IFileSystemHelper _fileSystemHelper;

    public PlayerRepository(IFileSystemHelper fileSystemHelper)
    {
       _fileSystemHelper = fileSystemHelper;
        _cachedPlayers= new Lazy<List<Player>>(LoadPlayers); 
    }
    
    
    public async Task<List<Player>>GetAll()
    { 
       return _cachedPlayers.Value;
    } 
    public async Task<Player> GetById(int id)
    { 
       return _cachedPlayers.Value.First(player => player.Id == id);
    } 
    private List<Player> LoadPlayers()
    { 
       bool exists = _fileSystemHelper.FileExists(FilePath);
       if (exists)
       {
          var jsonData= _fileSystemHelper.ReadAllText(FilePath);
          return JsonSerializer.Deserialize<List<Player>>(jsonData) ?? new List<Player>();
       } 
       return new List<Player>(); 
    }
}