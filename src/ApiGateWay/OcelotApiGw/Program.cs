using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingcontext, config0) =>
    {
        config0.AddJsonFile($"ocelot.{hostingcontext.HostingEnvironment.EnvironmentName}.json", true, true);
    });
builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
var app = builder.Build();
await app.UseOcelot();
app.Run();
