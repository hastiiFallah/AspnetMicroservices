using System;
using System.Threading.Tasks;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspnetRunBasics.Models;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IBasketService _basketService;

        public CheckOutModel(IOrderService orderService,IBasketService basketService)
        {
            _orderService = orderService;
            _basketService = basketService;
        }

        [BindProperty]
        public CheckoutModel OrderCheckOut { get; set; }

        public BasketModel Cart { get; set; } = new BasketModel();

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = "swn";
            Cart = await _basketService.GetBaket(userName);
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            var userName = "swn";
            Cart = await _basketService.GetBaket(userName);
            

            if (!ModelState.IsValid)
            {
                return Page();
            }

            OrderCheckOut.UserName = userName;
            OrderCheckOut.TotalPrice=Cart.TotalPrice;

            await _basketService.CheckoutBasket(OrderCheckOut);
            
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}