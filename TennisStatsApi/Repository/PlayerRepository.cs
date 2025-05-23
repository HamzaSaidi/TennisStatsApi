using System.Text.Json;
using TennisStatsApi.Exception;
using TennisStatsApi.Helpers;
using TennisStatsApi.Models;

namespace TennisStatsApi.Repository;

public class PlayerRepository:IPlayerRepository
{
   private readonly IWebHostEnvironment _env;

   private Lazy<List<Player>> _cachedPlayers;
       
     
    private IFileSystemHelper _fileSystemHelper;

    public PlayerRepository(IFileSystemHelper fileSystemHelper,IWebHostEnvironment env)
    {
       _fileSystemHelper = fileSystemHelper;
        _env = env;
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
       var filePath = Path.Combine(_env.ContentRootPath, "Data", "data.json"); 
       bool exists = _fileSystemHelper.FileExists(filePath);
       if (exists)
       {
          var jsonData= _fileSystemHelper.ReadAllText(filePath); 
          var options = new JsonSerializerOptions
          {
             PropertyNameCaseInsensitive = true // JSON keys can be camelCase or PascalCase
          };
          var data= JsonSerializer.Deserialize<Root>(jsonData,options) ;
          return data.Players;
       }
       
       throw new FileNotFoundException("file Not Found");
  
    }
}