using CatalogAPI.Models;
using MongoDB.Driver;

namespace CatalogAPI.Data
{
    public class CatelogContext : ICatelogContext
    {
        public CatelogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DataBaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DataBaseSettings:DataBaseName"));
            Products = database.GetCollection<Product>(configuration.GetValue<string>("DataBaseSettings:CollectionName"));
            SeedCategoryData.SeedCategory(Products);
                
        }
        public IMongoCollection<Product> Products { get; }
    }
}
