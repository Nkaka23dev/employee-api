using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace TheEmployeeAPI.Tests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public static TestSystemClock SystemClock { get; } = new TestSystemClock();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => 
        {
            var systemClockDescriptor = services.Single(d => d.ServiceType == typeof(ISystemClock));
            services.Remove(systemClockDescriptor);
            services.AddSingleton<ISystemClock>(SystemClock);
        });
    }
}

// public class CustomWebApplicationFactory: WebApplicationFactory<Program>
// {
//     protected override void ConfigureWebHost(IWebHostBuilder builder)
//     {
//         builder.ConfigureServices(services => {
//         var dbContextDescriptor = services.SingleOrDefault(
//             d => d.ServiceType == typeof(DbContextOptions<AppBbContext>));
//         if(dbContextDescriptor != null){
//             services.Remove(dbContextDescriptor);
//         }
        
//         var dbConnectionDescriptor = services.SingleOrDefault(
//          d => d.ServiceType == typeof(DbConnection));

//          if (dbConnectionDescriptor != null)
//          {
//              services.Remove(dbConnectionDescriptor);
//          }
         
//          //create open SQLite connection s o EF won't automatically close it.
//          services.AddSingleton<DbConnection>(container => {
//              var connection = new SqliteConnection("DataSource=:memory:");
//              connection.Open();

//              return connection;
//          });
//           services.AddDbContext<AppBbContext>((container, options) =>
//             {
//                 var connection = container.GetRequiredService<DbConnection>();
//                 options.UseSqlite(connection);
//             });
//           var sys
//         });

//     }
// }
public class TestSystemClock : ISystemClock
{
    public DateTimeOffset UtcNow { get; } = DateTimeOffset.Parse("2022-01-01T00:00:00Z");
}