using backend.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

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

app.MapGet("health", () => "Server is healthy");

app.MapGet("games", () => games);
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);

app.MapPost("games", (CreateGameDto newGame) =>
{
  Console.WriteLine(newGame);
  GameDto game = new(
    games.Count + 1,
    newGame.Name,
    newGame.Genre,
    newGame.Price,
    newGame.ReleaseDate
  );

  games.Add(game);

  return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);


});

app.Run();
