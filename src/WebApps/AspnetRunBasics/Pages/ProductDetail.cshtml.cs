using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;

namespace AspnetRunBasics
{
    public class ProductDetailModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly ICatelogService _catelogService;

        public ProductDetailModel(IBasketService basketService,ICatelogService catelogService)
        {
            _basketService = basketService;
            _catelogService = catelogService;
        }

        public CatalogModel Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            Product = await _catelogService.GetCatalog(productId);
            if (Product == null)
            {
                return NotFound();
            }
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