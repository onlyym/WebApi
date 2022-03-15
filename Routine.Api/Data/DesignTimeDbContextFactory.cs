using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Routine.Api.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RoutesDbContext>
    {
        public IConfiguration Configuration { get; }
        public RoutesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RoutesDbContext>();
            var sqlConnection = Configuration.GetConnectionString("SqlServerConnection");
            optionsBuilder.UseSqlite("Data Source=routine.db"); 

            return new RoutesDbContext(optionsBuilder.Options);
        }
    }
}
