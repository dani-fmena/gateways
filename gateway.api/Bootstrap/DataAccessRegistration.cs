using gateway.dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace gateway.api.Bootstrap
{
    /// <summary>
    /// Make the registration for data access, such as database, cache, non relational database, cdn, etc
    /// </summary>
    /// <remarks>This are extension method for IServiceCollection</remarks>
    public static class DataAccessRegistration
    {
        /// <summary>
        /// Register the data access layer services
        /// </summary>
        /// <param name="services">Service collection instance</param>
        /// <param name="conf">Configuration instance</param>
        public static void AddDataAccess(this IServiceCollection services, IConfiguration conf)
        {
            services.AddDbContext<ADbContext>(options =>
                options
                    .EnableSensitiveDataLogging()
                    .UseSqlServer(conf.GetConnectionString("Local")));
        }
    }
}