using Dapper;
using DiscountAPI.Models;
using Npgsql;

namespace DiscountAPI.Repository
{
    public class DiscountRepo : IDiscountRepo
    {
        private readonly IConfiguration _configuration;

        public DiscountRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> CreateCupon(Cupon cupon)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseConnection:ConnectionString"));
            var createdcupon = await connection.ExecuteAsync("INSERT INTO coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = cupon.ProductName, Description = cupon.Description, Amount = cupon.Amount });

            if (createdcupon == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteCupon(string productname)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseConnection:ConnectionString"));
            var cupon = await connection.ExecuteAsync("DELETE  FROM coupon WHERE ProductName = @ProductName", new { ProductName = productname });

            if (cupon == 0)
                return false;

            return true;
        }

        public async Task<Cupon> GetCupon(string productname)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseConnection:ConnectionString"));
            var cupon = await connection.QueryFirstOrDefaultAsync<Cupon>("SELECT * FROM coupon WHERE ProductName = @ProductName", new { ProductName = productname });

            if(cupon ==null)
                return new Cupon {ProductName="No product",Description="No desc",Amount=0};

            return cupon;
        }

        public async Task<bool?> UpdateCupon(Cupon cupon)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseConnection:ConnectionString"));
            var updatedcupon=await connection.ExecuteAsync("UPDATE coupon SET ProductName=@ProductName, Amount=@Amount, Description=@Description WHERE Id=@Id",
                new {ProductName=cupon.ProductName,Description=cupon.Description,Amount=cupon.Amount,Id=cupon.Id});

            if (updatedcupon == 0)
                return false;

            return true;
        }
    }
}
