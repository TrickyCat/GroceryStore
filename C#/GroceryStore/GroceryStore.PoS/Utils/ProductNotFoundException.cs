using System;

namespace GroceryStore.PoS.Utils
{
    public sealed class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message) { }
    }
}
