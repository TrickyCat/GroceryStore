using System.Threading.Tasks;
using GroceryStore.PoS.Cart;

namespace GroceryStore.PoS.PriceCalculation
{
    public interface IPriceCalculator
    {
        Task<decimal> CalculateAsync(IShoppingCart cart);
    }
}
