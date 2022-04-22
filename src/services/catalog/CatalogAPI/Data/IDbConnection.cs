using CatalogAPI.Models;
using MongoDB.Driver;

namespace CatalogAPI.Data
{
    public interface IDbConnection
    {
        MongoClient Client { get; }
        string DbName { get; }
        IMongoCollection<Product> ProductCollection { get; }
        string ProductCollectionName { get; }
    }
}