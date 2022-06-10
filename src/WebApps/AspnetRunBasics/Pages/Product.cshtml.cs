using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetRunBasics
{
    public class ProductModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly ICatelogService _catelogService;

        public ProductModel(IBasketService basketService, ICatelogService catelogService)
        {
            _basketService = basketService;
            _catelogService = catelogService;
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string? categoryName)
        {
            var product = await _catelogService.GetCatalog();
            CategoryList = product.Select(p => p.Category).Distinct();
            if (!string.IsNullOrEmpty(categoryName))
            {
                ProductList = product.Where(p => p.Category == categoryName);
                SelectedCategory = categoryName;
            }
            ProductList = product;

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await _catelogService.GetCatalog(productId);
            var userName = "swn";
            var basket = await _basketService.GetBaket(userName);

            basket.Items.Add(new BasketItemModel
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Color = "Black"

            });
            var updatedBasket = await _basketService.UpdateBasket(basket);
            return RedirectToPage("Cart");
        }
    }
}