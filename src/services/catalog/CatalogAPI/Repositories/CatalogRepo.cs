using CatalogAPI.Data;
using CatalogAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace CatalogAPI.Repositories
{
    public class CatalogRepo : ICatalogRepo
    {
        private readonly ICatelogContext _context;

        public CatalogRepo(ICatelogContext context)
        {
            _context = context;
        }
        public async Task CreateProduct(Product product)
        { 
            
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            if (filter == null)
            {
                return false;
            }
            else
            {
                var deletedResult = await _context.Products.DeleteOneAsync(filter);
                return deletedResult.IsAcknowledged &&
                    deletedResult.DeletedCount > 0;
            }
         
        }

        public async Task<IEnumerable<Product>> GetCategoryByName(string name)
        {
            
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, name);
            if (filter == null)
            {
                return null;
            }
            else
            {
                return await _context.Products.Find(filter).ToListAsync();
            }
            
        }

        public async Task<Product> GetProductById(string id)
        {
         
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                        .Products
                        .Find(p => true)
                        .ToListAsync();

        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updatedcontext =await _context.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, product);
            return updatedcontext.IsAcknowledged &&
                updatedcontext.ModifiedCount > 0;
        }
    }
}
