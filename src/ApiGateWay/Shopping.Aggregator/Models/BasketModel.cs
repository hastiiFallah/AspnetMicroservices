namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }
        public List<ExtendedBasketModel> Items { get; set; } = new List<ExtendedBasketModel>();
        public decimal TotalPrice { get; set; }
    }
}
