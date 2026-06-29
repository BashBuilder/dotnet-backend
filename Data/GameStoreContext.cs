using Microsoft.EntityFrameworkCore;


namespace backend.Data
{
  public class GameStoreContext(DbContextOptions<GameStoreContext> options)
    : DbContext(options)
  {

  }
}