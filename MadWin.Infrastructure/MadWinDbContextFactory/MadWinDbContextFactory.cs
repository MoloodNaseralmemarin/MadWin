using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MadWin.Infrastructure.Context
{
    public class MadWinDbContextFactory : IDesignTimeDbContextFactory<MadWinDBContext>
    {
        public MadWinDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // ← محل EF CLI
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MadWinDBContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Production"));

            return new MadWinDBContext(optionsBuilder.Options);
        }
    }
}
