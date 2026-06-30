using backend.Entities;
using Microsoft.EntityFrameworkCore;


namespace backend.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options)
  : DbContext(options)
{
  public DbSet<Game> Games => Set<Game>();

  public DbSet<Genre> Genres => Set<Genre>();
}
