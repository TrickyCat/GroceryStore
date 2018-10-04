using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStore.PoS.Products
{
    public class FakeProductOfferRepository : IProductOfferRepository
    {
        private static readonly Dictionary<string, List<ProductOffer>> Offers;
        private static readonly IReadOnlyList<ProductOffer> EmptyOffers = new List<ProductOffer>().AsReadOnly();

        static FakeProductOfferRepository()
        {
            Offers = new Dictionary<string, List<ProductOffer>>(StringComparer.InvariantCultureIgnoreCase);
            AddOffer(
                new ProductOffer(code: "OFFER_AVOCADO_FALL_2018", title: "Incredible chance to buy avocados.", offerPrice: 3m, productCode: "A", productQty: 3)
                );
            AddOffer(
                new ProductOffer(code: "OFFER_CARROT_FALL_2018", title: "Buy more carrots for less money.", offerPrice: 5m, productCode: "C", productQty: 6)
                );
        }

        private static void AddOffer(ProductOffer offer)
        {
            var lst = Offers.ContainsKey(offer.ProductCode)
                ? Offers[offer.ProductCode]
                : new List<ProductOffer>();

            lst.Add(offer); // no checks for same offering because of 'Fake' nature of this repository
            Offers[offer.ProductCode] = lst;
        }

        public Task<IReadOnlyList<ProductOffer>> GetByProductCodeAsync(string productCode)
        {
            return Task.FromResult(
                Offers.ContainsKey(productCode) ? Offers[productCode].AsReadOnly() : EmptyOffers
                );
        }
    }
}
