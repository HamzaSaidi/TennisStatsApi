using System.Text.Json;
using TennisStatsApi.Exception;
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
    
    
    public async Task<IQueryable<Player>> GetAll()
    { 
       return _cachedPlayers.Value.AsQueryable();
    } 
    public async Task<Player> GetById(int id)
    {
       try
       {
          return _cachedPlayers.Value.First(player => player.Id == id);

       }
       catch (System.Exception e)
       {
           
          throw new NotFoundException("Player Not found");
       }
    } 
    private List<Player> LoadPlayers()
    { 
       bool exists = _fileSystemHelper.FileExists(FilePath);
       if (exists)
       {
          var jsonData= _fileSystemHelper.ReadAllText(FilePath);
          return JsonSerializer.Deserialize<List<Player>>(jsonData) ?? new List<Player>();
       }

       throw new InvalidOperationException("Internal server error");
    }
}