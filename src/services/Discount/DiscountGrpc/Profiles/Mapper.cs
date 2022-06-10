using AutoMapper;
using DiscountGrpc.Models;
using DiscountGrpc.Protos;

namespace DiscountGrpc.Profiles
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Cupon, CuponModel>().ReverseMap();
        }
    }
}
