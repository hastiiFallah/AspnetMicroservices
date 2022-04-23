using CatalogAPI.Models;
using MongoDB.Driver;

namespace CatalogAPI.Data
{
    public interface ICatelogContext
    {
        public IMongoCollection<Product> Products { get; }
    }
}
