using System.Threading.Tasks;

namespace GroceryStore.PoS.Products
{
    // Repositories will involve some IO so we'll use async operations
    public interface IProductRepository
    {
        Task<Product> GetByCodeAsync(string productCode);
    }
}
