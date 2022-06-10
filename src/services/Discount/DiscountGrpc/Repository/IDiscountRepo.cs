using DiscountGrpc.Models;
using System.Threading.Tasks;

namespace DiscountGrpc.Repository
{
    public interface IDiscountRepo
    {
        Task<Cupon> GetCupon(string productname);
        Task<bool> DeleteCupon(string productname);
        Task<bool?> UpdateCupon( Cupon cupon);
        Task<bool> CreateCupon(Cupon cupon);
    }
}
