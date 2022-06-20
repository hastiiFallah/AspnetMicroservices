
using DiscountGrpc.Repository;
using DiscountGrpc.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscountGrpc
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddScoped<IDiscountRepo, DiscountRepo>();
            services.AddAutoMapper(typeof(Startup));
            services.AddHealthChecks()
                .AddNpgSql("Server=localhost;Port=5432;Database=DiscountDb;User Id=admin;Password=admin1234;");
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DiscountService>();
                endpoints.MapHealthChecks("/hc",new HealthCheckOptions()
                {
                    Predicate=_=>true,
                    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
