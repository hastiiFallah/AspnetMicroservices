using DiscountAPI.Models;

namespace DiscountAPI.Repository
{
    public interface IDiscountRepo
    {
        Task<Cupon> GetCupon(string productname);
        Task<bool> DeleteCupon(string productname);
        Task<bool?> UpdateCupon( Cupon cupon);
        Task<bool> CreateCupon(Cupon cupon);
    }
}
