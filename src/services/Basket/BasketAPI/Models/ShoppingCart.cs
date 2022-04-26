namespace BasketAPI.Models
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {

        }
        public ShoppingCart(string username)
        {
            UserName=username;
        }
        public decimal TotalPrice
        {
            get
            {
                decimal price = 0;
                foreach (var item in items)
                {
                    price += item.Price * item.Quantity;
                }
                return price;
            }
        }
    }
}
