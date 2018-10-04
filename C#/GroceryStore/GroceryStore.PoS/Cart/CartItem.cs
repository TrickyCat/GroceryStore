using System;

namespace GroceryStore.PoS.Cart
{
    public sealed class CartItem: ICartItem
    {
        public string Code { get; }

        public CartItem(string code)
        {
            Code = !string.IsNullOrEmpty(code) ? code : throw new ArgumentOutOfRangeException($"[{nameof(code)}] parameter is empty.");
        }

        public override string ToString() => $"[{Code}]";
    }
}
