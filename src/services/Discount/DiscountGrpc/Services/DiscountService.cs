using AutoMapper;
using DiscountGrpc.Models;
using DiscountGrpc.Protos;
using DiscountGrpc.Repository;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DiscountGrpc.Services
{
    public class DiscountService: DiscountprotoService.DiscountprotoServiceBase 
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        private readonly IDiscountRepo _repo;

        public DiscountService(ILogger<DiscountService>logger,IMapper mapper,IDiscountRepo repo)
        {
            _logger = logger;
            _mapper = mapper;
            _repo = repo;
        }
        public override async Task<CuponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var cupon=await _repo.GetCupon(request.ProductName);
            if (cupon == null)
            {
                throw new RpcException((new Status(StatusCode.NotFound, $"Discount with Product Name={request.ProductName}Not Fount")));
            }
            var cuponModel=_mapper.Map<CuponModel>(cupon);
            return cuponModel;
        }
        public override async Task<CuponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var cupon = _mapper.Map<Cupon>(request.Cupon);
            await _repo.CreateCupon(cupon);

            var cuponmodel = _mapper.Map<CuponModel>(cupon);
            return cuponmodel;
        }
        public override async Task<CuponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var cupon = _mapper.Map<Cupon>(request.Cupon);
            await _repo.UpdateCupon(cupon);

            var cuponmodel=_mapper.Map<CuponModel>(cupon);
            return cuponmodel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deletedcupon = await _repo.DeleteCupon(request.ProductName);
            var respone = new DeleteDiscountResponse
            {
                Success = true,
            };
            return respone;
        }
    }
}
