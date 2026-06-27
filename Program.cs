using backend.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
  new(
    1,
    "Street Fighter II",
    "Fighting",
    19.99M,
    new DateOnly(1992, 6, 12)
  ),
  new(
    2,
    "Final Fantasy",
    "Role Playing",
    49.99M,
    new DateOnly(1992, 6, 10)
  ),
  new(
    3,
    "Fifa 23",
    "Sports",
    49.99M,
    new DateOnly(2002, 1, 10)
  ),
];

app.MapGet("/", () => "Hello World from a c# application");

app.MapGet("games", () => games);

app.Run();
