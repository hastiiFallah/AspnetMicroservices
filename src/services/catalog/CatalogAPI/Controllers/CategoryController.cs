using CatalogAPI.Models;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICatalogRepo _catelog;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICatalogRepo catelog,ILogger<CategoryController> logger)
        {
            _catelog = catelog;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var product = await _catelog.GetProducts();
            return Ok(product);
        }
        [HttpGet("{id}",Name = "GetProduct")]
        public async Task <ActionResult<Product>>GetProductByID(string id)
        {
            var product=await _catelog.GetProductById(id);
            if(product == null)
            {
                _logger.LogError($"catelog with id:{id} Not Found");
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Product>>>GetProductByCategoy(string name)
        {
            var category=await _catelog.GetCategoryByName(name);
            return Ok(category);    
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _catelog.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new {id=product.Id},product);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _catelog.UpdateProduct(product));
        }
        [HttpDelete("{id")]
        public async Task<IActionResult>DeleteProduct(string id)
        {
            return Ok(await _catelog.DeleteProduct(id));
        }
    }
}
