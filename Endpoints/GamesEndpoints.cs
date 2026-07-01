using backend.Data;
using backend.Dtos;
using backend.Entities;

namespace backend.Endpoints
{
  public static class GamesEndpoints
  {
    const string GetGameEndpointName = "GetGame";

    private readonly static List<GameDto> games = [
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

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
      var group = app.MapGroup("games")
                     .WithParameterValidation();


      group.MapGet("/", () => games);
      group.MapGet("/{id}", (int id) =>
      {
        GameDto? game = games.Find(game => game.Id == id);

        return game is null ? Results.NotFound() : Results.Ok(game);

      }).WithName(GetGameEndpointName);


      group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
      {
        Game game = new()
        {
          Name = newGame.Name,
          Genre = dbContext.Genres.Find(newGame.GenreId),
          GenreId = newGame.GenreId,
          Price = newGame.Price,
          ReleaseDate = newGame.ReleaseDate
        };

        dbContext.Games.Add(game);
        dbContext.SaveChanges();

        GameDto gameDto = new(
          game.Id,
          game.Name,
          game.Genre!.Name,
          game.Price,
          game.ReleaseDate
        );

        return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
      });

      group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
      {
        int index = games.FindIndex(game => game.Id == id);

        if (index == -1)
        {
          return Results.NotFound();
        }

        games[index] = new(
          id,
          updatedGame.Name,
          updatedGame.Genre,
          updatedGame.Price,
          updatedGame.ReleaseDate
        );

        return Results.NoContent();
      });

      group.MapDelete("/{id}", (int id) =>
      {
        games.RemoveAll(game => game.Id == id);

        return Results.NoContent();
      });
      return group;
    }


  }
}