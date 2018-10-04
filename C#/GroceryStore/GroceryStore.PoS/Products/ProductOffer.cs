using System;

namespace GroceryStore.PoS.Products
{
    public sealed class ProductOffer
    {
        public string Code { get; }
        public string Title { get; }
        public decimal OfferPrice { get; }

        public string ProductCode { get; }
        public int ProductQty { get; }

        public ProductOffer(string code, string title, decimal offerPrice, string productCode, int productQty)
        {
            Code = !string.IsNullOrEmpty(code) ? code : throw new ArgumentOutOfRangeException($"[{nameof(code)}] parameter is empty.");
            Title = !string.IsNullOrEmpty(title) ? title : throw new ArgumentOutOfRangeException($"[{nameof(title)}] parameter is empty.");
            // Yep! Our store offers some product bundles for free occasionally
            OfferPrice = offerPrice >= 0 ? offerPrice : throw new ArgumentOutOfRangeException($"[{nameof(offerPrice)}] parameter is negative.");

            ProductCode = !string.IsNullOrEmpty(productCode) ? productCode : throw new ArgumentOutOfRangeException($"[{nameof(productCode)}] parameter is empty.");
            ProductQty = productQty > 0 ? productQty : throw new ArgumentOutOfRangeException($"[{nameof(productQty)}] parameter is not positive."); ;
        }

        public sealed class MatchResult
        {
            public bool IsMatch { get; }
            public string OfferCode { get; }
            public decimal OfferPrice { get; }
            public string ProductCode { get; }
            public int ProductQty { get; }
            public int RemainingProductQty { get; }

            public MatchResult(bool isMatch, string offerCode, decimal offerPrice, string productCode, int productQty, int remainingProductQty)
            {
                IsMatch = isMatch;
                OfferCode = offerCode;
                ProductCode = productCode;
                ProductQty = productQty;
                RemainingProductQty = remainingProductQty;
                OfferPrice = offerPrice;
            }
        }

        public MatchResult Matches(string productCode, int productQty)
        {
            var isMatch = string.Equals(productCode, ProductCode, StringComparison.InvariantCultureIgnoreCase)
                          && productQty >= ProductQty;
            return new MatchResult(isMatch, Code, OfferPrice, ProductCode, productQty, productQty - ProductQty);
        }
    }
}
