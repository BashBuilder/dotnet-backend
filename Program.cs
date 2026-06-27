using backend.Dtos;
using backend.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("health", () => "Server is healthy");
app.MapGamesEndpoints();


app.Run();
