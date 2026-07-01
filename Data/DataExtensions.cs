using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
  public static class DataExtensions
  {
    public static void MigrateDb(this WebApplication application)
    {
      using var scope = application.Services.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

      dbContext.Database.Migrate();
    }
  }
}