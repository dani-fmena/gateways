using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

using gateway.api.Bootstrap;
using gateway.api.V1.Mapping;

namespace gateway.api
{
    public class Startup
    {
        private IConfiguration _conf { get; }
        
        public Startup(IConfiguration conf)
        {
            _conf = conf;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsDevPolicy();
            services.AddControllers();
            services.AddAutoMapper(typeof(Mapping));
            services.AddDataAccess(_conf);
            services.AddBusinessLogic();
            services.AddApiDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVer)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            
            app.UseFileServer(new FileServerOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/statics",
                EnableDefaultFiles = true
            });
            
            app.UseRouting();
            // app.UseHttpsRedirection();
            app.UseCors("CORS-Policy");
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });        // Allow to middleware(s) to knows HTTP matches requests to endpoints, earlier en the pipeline process
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.InjectStylesheet("/statics/SwaggerDark.css");
                    options.DefaultModelsExpandDepth(-1);
                    foreach (var description in apiVer.ApiVersionDescriptions)
                        options.SwaggerEndpoint( $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                });
        }
    }
}