using CatalogAPI.Models;
using MongoDB.Driver;

namespace CatalogAPI.Data
{
    public class DbConnection : IDbConnection
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase _db;
        private string _connectionid = "MongoDB";
        public string DbName { get; private set; }
        public string ProductCollectionName { get; private set; } = "products";
        public MongoClient Client { get; private set; }
        public IMongoCollection<Product> ProductCollection { get; private set; }

        public DbConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            Client = new MongoClient(_configuration.GetConnectionString(_connectionid));
            DbName = _configuration["DatabaseName"];
            _db = Client.GetDatabase(DbName);

            ProductCollection = _db.GetCollection<Product>(ProductCollectionName);
        }
    }
}
