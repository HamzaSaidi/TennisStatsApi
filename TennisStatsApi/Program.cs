using TennisStatsApi.Helpers;
using TennisStatsApi.Repository;
using TennisStatsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IFileSystemHelper, FileSystemHelper>();
builder.Services.AddSingleton<IPlayerRepository, PlayerRepository>(); 
builder.Services.AddSingleton<IPlayerService, PlayerService>(); 
var app = builder.Build(); 
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();
 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();