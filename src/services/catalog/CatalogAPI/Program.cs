using CatalogAPI.Data;
using CatalogAPI.Repositories;
using CommonLogging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Host.CreateDefaultBuilder(args)
    .UseSerilog(Serilogger.Configure);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICatelogContext, CatelogContext>();
builder.Services.AddScoped<ICatalogRepo, CatalogRepo>();

builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration["DataBaseSettings:ConnectionString"],
    "Catelog MongoDB Health",
    HealthStatus.Degraded);

var app = builder.Build();

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
