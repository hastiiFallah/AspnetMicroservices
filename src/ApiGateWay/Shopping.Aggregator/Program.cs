using CommonLogging;
using Serilog;
using Shopping.Aggregator.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);
Host.CreateDefaultBuilder(args)
    .UseSerilog(Serilogger.Configure);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<LoggingDelegatingHandler>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3,_=> TimeSpan.FromSeconds(2)))
    .AddTransientHttpErrorPolicy(policy=>policy.CircuitBreakerAsync(3,TimeSpan.FromSeconds(50)));

builder.Services.AddHttpClient<ICatelogService, CatelogService>(c =>
c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
