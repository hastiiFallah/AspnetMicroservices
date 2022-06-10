using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatelogService _catelogService;
        private readonly IBasketService _basketService;

        public IndexModel(ICatelogService catelogService,IBasketService basketService)
        {
            _catelogService = catelogService;
            _basketService = basketService;
        }
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _catelogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await _catelogService.GetCatalog(productId);
            var userName = "swn";
            var basket = await _basketService.GetBaket(userName);

            basket.Items.Add(new BasketItemModel
            {
                ProductId=productId,
                ProductName=product.Name,
                Price=product.Price,
                Quantity=1,
                Color="Black"
                
            });
            var updatedBasket = await _basketService.UpdateBasket(basket);
            return RedirectToPage("Cart");
        }
    }
}
