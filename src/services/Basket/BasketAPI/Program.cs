using BasketAPI.gRPC_Services;
using BasketAPI.Rrpo;
using DiscountGrpc.Protos;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpcClient<DiscountprotoService.DiscountprotoServiceClient>(o => o.Address = new Uri("http://localhost:5003"));
builder.Services.AddScoped<DiscountGrpcServices>();
builder.Services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();


builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(configuration["EventBusSetting:HostService"]);
    });
});
builder.Services.AddMassTransitHostedService();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetValue<string>("CacheSettings:connectionstring");
});
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
