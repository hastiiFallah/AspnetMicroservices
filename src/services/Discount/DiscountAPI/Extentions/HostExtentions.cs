using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;

namespace DiscountAPI.Extensions
{
    public static class HostExtentions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postresql database.");

                    var retry = Policy.Handle<NpgsqlException>()
                        .WaitAndRetry(
                        retryCount: 5,
                        sleepDurationProvider: retryAttampt => TimeSpan.FromSeconds(Math.Pow(2, retryAttampt)),
                        onRetry: (exception, retryCount, context) =>
                        {
                            logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                        });


                    retry.Execute(() => ExecuteMigration(configuration));

                    

                    logger.LogInformation("Migrated postresql database.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");

                   
                }
            }

            return host;
        }

        private static void ExecuteMigration(IConfiguration configuration)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();

            using var command = new NpgsqlCommand
            {
                Connection = connection
            };
          
            command.CommandText = "DROP TABLE IF EXISTS coupon";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE TABLE coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
            command.ExecuteNonQuery();
        }
    }
}
