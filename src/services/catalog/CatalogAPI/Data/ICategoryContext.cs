using CatalogAPI.Models;
using MongoDB.Driver;

namespace CatalogAPI.Data
{
    public interface ICategoryContext
    {
        public IMongoCollection<Product> Products { get; }
    }
}
