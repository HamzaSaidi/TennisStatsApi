 
using TennisStatsApi.Helpers;
using TennisStatsApi.Middlewares;
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
builder.Services.AddSingleton<IStatsService, StatsService>(); 
  
var app = builder.Build(); 
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI(); 

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }

    await next();
});

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();

app.MapControllers();

app.Run();
