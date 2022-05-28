using gateway.api.V1.Services;
using Microsoft.Extensions.DependencyInjection;


namespace gateway.api.Bootstrap
{
    /// <summary>
    /// Make the registration for business logic services
    /// </summary>
    /// <remarks>This are extension method for IServiceCollection</remarks>
    public static class BusinessLogicRegistration
    {
        /// <summary>
        /// Register the business logic as services
        /// </summary>
        /// <param name="services">Service collection instance</param>
        public static void AddBusinessLogic(this IServiceCollection services)
        {
            services.AddTransient<ISvcGateway, SvcGateway>();
            services.AddTransient<ISvcPeripherals, SvcPeripherals>();
        }
    }
}