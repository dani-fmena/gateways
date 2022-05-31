using Microsoft.Extensions.DependencyInjection;


namespace gateway.api.Bootstrap
{
    public static class CorsExt
    {
        /// <summary>
        /// Configures CORS headers and preflight requests on server side.
        /// Allows for a development server from different port, to submit
        /// requests to the API
        /// </summary>
        /// <param name="services">Program service collection interface</param>
        public static void AddCorsDevPolicy(this IServiceCollection services)
        {
            // CORS service registration.
            // Right now it is plain simple cors policy for a development server. If this policies grows bigger
            // then maybe it should be parametrized, or get a list of hosts from a config file
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CORS-Policy", builder =>
                {
                    // VUE App dev server
                    builder
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .WithOrigins("http://localhost:9000");
                });
            });
        }
    }
}
