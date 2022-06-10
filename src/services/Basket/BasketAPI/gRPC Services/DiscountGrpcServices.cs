using DiscountGrpc.Protos;

namespace BasketAPI.gRPC_Services
{
    public class DiscountGrpcServices
    {
        private readonly DiscountprotoService.DiscountprotoServiceClient _discountprotoService;

        public DiscountGrpcServices(DiscountprotoService.DiscountprotoServiceClient discountprotoService)
        {
            _discountprotoService = discountprotoService;
        }
        public async Task<CuponModel> GetDiscountAsync(string productname)
        {
            var discount = new GetDiscountRequest { ProductName=productname };
            return await _discountprotoService.GetDiscountAsync(discount);
        }
    }
}
