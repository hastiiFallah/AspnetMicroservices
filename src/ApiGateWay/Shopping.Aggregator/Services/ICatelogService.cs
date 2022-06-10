using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface ICatelogService
    {
        Task<IEnumerable<CatelogModel>> GetCatelog();
        Task<IEnumerable<CatelogModel>> GetCatelogByCategory(string categoryName);
        Task<CatelogModel> GetCatelog(string id);
    }
}
