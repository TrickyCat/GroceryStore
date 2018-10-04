using System.Threading.Tasks;
using GroceryStore.PoS.Cart;
using GroceryStore.PoS.PriceCalculation;

namespace GroceryStore.PoS.PointOfSale
{
    public sealed class SimplePosTerminal: IPosTerminal
    {
        private readonly IPriceCalculator _priceCalculator;
        private readonly IShoppingCart _shoppingCart;

        public SimplePosTerminal(IPriceCalculator priceCalculator, IShoppingCart shoppingCart)
        {
            _priceCalculator = priceCalculator;
            _shoppingCart = shoppingCart;
        }

        public void ScanItemCode(string code) => _shoppingCart.Add(new CartItem(code));

        public void RemoveItemCode(string code) => _shoppingCart.Remove(new CartItem(code));

        public Task<decimal> CalculateTotalAsync() => _priceCalculator.CalculateAsync(_shoppingCart);

        public override string ToString() => _shoppingCart.ToString();
    }
}
