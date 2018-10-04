using System;

namespace GroceryStore.PoS.Products
{
    public sealed class Product
    {
        public string Code { get; }
        public string Title { get; }
        public decimal UnitPrice { get; }

        public Product(string code, string title, decimal unitPrice)
        {
            Code = !string.IsNullOrEmpty(code) ? code : throw new ArgumentOutOfRangeException($"[{nameof(code)}] parameter is empty.");
            Title = !string.IsNullOrEmpty(title) ? title : throw new ArgumentOutOfRangeException($"[{nameof(title)}] parameter is empty.");
            // Yep! Our store offers some products for free occasionally
            UnitPrice = unitPrice >= 0 ? unitPrice : throw new ArgumentOutOfRangeException($"[{nameof(unitPrice)}] parameter is negative.");
        }

        public override string ToString() => $"[{Code}] {Title}   Unit price: {UnitPrice}";
    }
}
