using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStore.PoS.Products
{
    public interface IProductOfferRepository
    {
        Task<IReadOnlyList<ProductOffer>> GetByProductCodeAsync(string productCode);
    }
}
