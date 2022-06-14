using CatalogAPI.Data;
using CatalogAPI.Repositories;
using CommonLogging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Host.CreateDefaultBuilder(args)
    .UseSerilog(Serilogger.Configure);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICatelogContext, CatelogContext>();
builder.Services.AddScoped<ICatalogRepo, CatalogRepo>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
