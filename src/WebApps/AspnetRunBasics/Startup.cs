
using AspnetRunBasics.Services;
using CommonLogging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using System;
using System.Net.Http;

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
            services.AddHealthChecks()
                .AddUrlGroup(new Uri($"{Configuration["ApiSettings:GateWayAddress"]}"), "Ocelot GW Health", HealthStatus.Degraded);

            services.AddTransient<LoggingDelegatingHandler>();
            services.AddHttpClient<ICatelogService, CatalogService>(c =>
            c.BaseAddress = new Uri(Configuration["ApiSettings:GateWayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                .AddPolicyHandler(RetryPolicy())
                .AddPolicyHandler(CircuteBreaker());

            services.AddHttpClient<IBasketService, BasketService>(c =>
            c.BaseAddress = new Uri(Configuration["ApiSettings:GateWayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                 .AddPolicyHandler(RetryPolicy())
                .AddPolicyHandler(CircuteBreaker());

            services.AddHttpClient<IOrderService, OrderService>(c =>
            c.BaseAddress = new Uri(Configuration["ApiSettings:GateWayAddress"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>()
                 .AddPolicyHandler(RetryPolicy())
                .AddPolicyHandler(CircuteBreaker());

            services.AddRazorPages();
        }
        private static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryattampt => TimeSpan.FromSeconds(Math.Pow(2, retryattampt)),
                onRetry: (retryCount, exeption, context) =>
                {
                    Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey} failed due {exeption}");
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> CircuteBreaker()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(2)
                );
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
                endpoints.MapHealthChecks("/hc",new HealthCheckOptions()
                {
                    Predicate=_=>true,
                    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
