using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryStore.PoS.Cart
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly List<ICartItem> _items = new List<ICartItem>();

        public IEnumerable<ICartItem> Items => _items;

        public void Add(ICartItem item)
        {
            _items.Add(item);
        }

        public void Remove(ICartItem item)
        {
            var itemToRemove = _items.Find(i => string.Equals(i.Code, item.Code, StringComparison.InvariantCultureIgnoreCase));
            if (itemToRemove != null)
            {
                _items.Remove(itemToRemove);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Shopping Cart:");
            sb.Append("[ " + string.Join(" , ", _items.Select(i => i.ToString())) + " ]");
            return sb.ToString();
        }
    }
}
