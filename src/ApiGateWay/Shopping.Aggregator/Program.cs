using CommonLogging;
using Serilog;
using Shopping.Aggregator.Services;
using Polly;
using Polly.Extensions.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
Host.CreateDefaultBuilder(args)
    .UseSerilog(Serilogger.Configure);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:BasketUrl"]}/swagger/index.html"), "BasketApi", HealthStatus.Degraded)
    .AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:CatalogUrl"]}/swagger/index.html"), "CatelogApi", HealthStatus.Degraded)
    .AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:OrderingUrl"]}/swagger/index.html"), "OrderApi", HealthStatus.Degraded);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(RetryPolicy())
    .AddPolicyHandler(CircuteBreaker());

builder.Services.AddHttpClient<ICatelogService, CatelogService>(c =>
c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(RetryPolicy())
    .AddPolicyHandler(CircuteBreaker());

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(RetryPolicy())
    .AddPolicyHandler(CircuteBreaker());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
           retryCount: 5,
           sleepDurationProvider: retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)),
           onRetry: (exeption, retrycount, context) =>
           {
               Log.Error($"Retry {retrycount} of {context.PolicyKey} at {context.OperationKey} failed due {exeption.Exception}");
           });

}

static IAsyncPolicy<HttpResponseMessage> CircuteBreaker()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
           handledEventsAllowedBeforeBreaking: 5,
           durationOfBreak:  TimeSpan.FromSeconds(30)
          );

}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/hc",new HealthCheckOptions()
{
    Predicate=_=>true,
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
