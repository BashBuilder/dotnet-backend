using backend.Data;
using backend.Dtos;
using backend.Entities;
using backend.Mapping;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints
{
  public static class GamesEndpoints
  {
    const string GetGameEndpointName = "GetGame";

    private readonly static List<GameSummaryDto> games = [
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

      // get all games
      group.MapGet("/", async (GameStoreContext dbContext) =>
        await dbContext
          .Games.Include(game => game.Genre)
          .Select(game => game.ToGameSummaryDto())
          .AsNoTracking()
          .ToListAsync()
        );


      // get games by id
      group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
      {

        // GameDto? game = games.Find(game => game.Id == id);
        Game? game = await dbContext.Games.FindAsync(id);

        return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

      }).WithName(GetGameEndpointName);


      group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
      {
        Game game = newGame.ToEntity();
        // game.Genre = dbContext.Genres.Find(newGame.GenreId);
        // Game game = new()
        // {
        //   Name = newGame.Name,
        //   Genre = dbContext.Genres.Find(newGame.GenreId),
        //   GenreId = newGame.GenreId,
        //   Price = newGame.Price,
        //   ReleaseDate = newGame.ReleaseDate
        // };

        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();

        // GameDto gameDto = new(
        //   game.Id,
        //   game.Name,
        //   game.Genre!.Name,
        //   game.Price,
        //   game.ReleaseDate
        // );

        return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameSummaryDto());
      });

      // update game by id
      group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
      {
        // int index = games.FindIndex(game => game.Id == id);

        // if (index == -1)
        // {
        //   return Results.NotFound();
        // }

        Game? game = await dbContext.Games.FindAsync(id);
        if (game is null)
        {
          return Results.NotFound();
        }

        // games[index] = new(
        //   id,
        //   updatedGame.Name,
        //   updatedGame.Genre,
        //   updatedGame.Price,
        //   updatedGame.ReleaseDate
        // );

        dbContext.Entry(game)
          .CurrentValues
          .SetValues(updatedGame.ToEntity(id));

        await dbContext.SaveChangesAsync();

        return Results.NoContent();
      });

      group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
      {
        // Game? game = dbContext.Games.Find(id);
        // if (game is null)
        // {
        //   return Results.NotFound();
        // }

        // dbContext.Games.Remove(game);

        await dbContext.Games
          .Where(game => game.Id == id)
          .ExecuteDeleteAsync();
        // dbContext.SaveChanges();

        return Results.NoContent();
      });
      return group;
    }


  }
}