
using AspnetRunBasicBlazor.Services;
using CommonLogging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspnetRunBasics
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<LoggingDelegatingHandler>();
            services.AddHttpClient<ICatalogService, CatalogService>(c =>
            c.BaseAddress = new Uri(Configuration["ApiSettings:GateWayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();

            services.AddHttpClient<IBasketService, BasketService>(c =>
            c.BaseAddress = new Uri(Configuration["ApiSettings:GateWayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();

            services.AddHttpClient<IOrderService, OrderService>(c =>
            c.BaseAddress = new Uri(Configuration["ApiSettings:GateWayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();

            services.AddHealthChecks()
                 .AddUrlGroup(new Uri($"{Configuration["ApiSettings:GateWayAddress"]}"), "OcelotGW Health", HealthStatus.Degraded);

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
