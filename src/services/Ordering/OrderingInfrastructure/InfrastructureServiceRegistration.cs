using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderingApplication.Contracts.Infrastructure;
using OrderingApplication.Contracts.Persistense;
using OrderingApplication.Models;
using OrderingInfrastructure.Mail;
using OrderingInfrastructure.Persistence;
using OrderingInfrastructure.Repositories;

namespace OrderingInfrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));

            services.AddScoped(typeof(IAsyncRepo<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepo, OrderRepository>();

            services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
