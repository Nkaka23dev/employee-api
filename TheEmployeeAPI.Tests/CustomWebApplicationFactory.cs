using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TheEmployeeAPI.Tests;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => {
        var dbContextDescriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<AppBbContext>));
        services.Remove(dbContextDescriptor);

        var dbConnectionDescriptor = services.SingleOrDefault(
         d => d.ServiceType == typeof(DbConnection));

         services.Remove(dbConnectionDescriptor);
         
         //create open SQLite connection so EF won't automatically close it.
         services.AddSingleton<DbConnection>(container => {
             var connection = new SqliteConnection("DataSource=:memory:");
             connection.Open();

             return connection;
         });
          services.AddDbContext<AppBbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });

    }
}
