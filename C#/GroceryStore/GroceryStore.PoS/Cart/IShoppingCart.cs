using System.Collections.Generic;

namespace GroceryStore.PoS.Cart
{
    public interface IShoppingCart
    {
        IEnumerable<ICartItem> Items { get; }
        void Add(ICartItem item);
        void Remove(ICartItem item);
    }
}
