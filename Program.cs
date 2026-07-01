using backend.Data;
using backend.Dtos;
using backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connectionString);

var app = builder.Build();

app.MapGet("health", () => "Server is healthy");

app.MapGamesEndpoints();

await app.MigrateDbAsync();


app.Run();
