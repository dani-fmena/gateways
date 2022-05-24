using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using gateway.dal;
using Microsoft.Data.SqlClient;

namespace gateway.factory
{
    /// <summary>
    /// We need this so EF can access our custom DbContext on design time, eg. when using <b>ef</b> commands
    /// from the CLI / console 
    /// </summary>
    public class DbContextFactory  : IDesignTimeDbContextFactory<ADbContext>
    {
        public ADbContext CreateDbContext(string[] args)
        {
            // getting the conf project file
            IConfiguration _conf = new ConfigurationBuilder().AddJsonFile("settings.development.json", optional: false, reloadOnChange: true).Build();

            // creating the DbContext option builder instance
            var optionsBuilder = new DbContextOptionsBuilder<ADbContext>();
            optionsBuilder.UseSqlServer(
                new SqlConnection(_conf.GetConnectionString("Gateway")),
                b => b.MigrationsAssembly("gateway.factory")
            );

            // DbContext instantiation with prev custom option, ready for design time
            return new ADbContext(optionsBuilder.Options);
        }
    }
}