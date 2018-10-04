using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryStore.PoS.Cart;
using GroceryStore.PoS.Products;
using GroceryStore.PoS.Utils;

namespace GroceryStore.PoS.PriceCalculation
{
    public class SimplePriceCalculator : IPriceCalculator
    {
        private readonly IProductOfferRepository _offersRepository;
        private readonly IProductRepository _productRepository;

        public SimplePriceCalculator(IProductOfferRepository offersRepository,
            IProductRepository productRepository)
        {
            _offersRepository = offersRepository;
            _productRepository = productRepository;
        }

        public async Task<decimal> CalculateAsync(IShoppingCart cart)
        {
            var productPrices = await cart.Items
                .GroupBy(p => p.Code.ToLowerInvariant())
                .SelectAndWaitAll(g => CalculateForProductAsync(g.Key, g.Count()))
                .ConfigureAwait(false);

            return productPrices.Sum();
        }

        private async Task<decimal> CalculateForProductAsync(string productCode, int productQty)
        {
            var productOffers = await _offersRepository.GetByProductCodeAsync(productCode).ConfigureAwait(false);
            return productOffers.Any()
                ? await CalculateForProductWithOffersAsync(productCode, productQty, productOffers).ConfigureAwait(false)
                : await CalculateForProductWithoutOffersAsync(productCode, productQty).ConfigureAwait(false);
        }

        private async Task<decimal> CalculateForProductWithOffersAsync(string productCode, int productQty, IReadOnlyList<ProductOffer> productOffers)
        {
            var (remainingProductQty, matchedOffers) = productOffers.Aggregate(
                (remainingQty: productQty, offers: new Dictionary<ProductOffer, int>()),
                (acc, offer) =>
                {
                    var matchResult = offer.Matches(productCode, productQty);
                    if (matchResult.IsMatch)
                    {
                        var (remainingQty, offers) = acc;
                        if (!offers.ContainsKey(offer))
                        {
                            offers[offer] = 0;
                        }

                        do
                        {
                            remainingQty = matchResult.RemainingProductQty;
                            offers[offer]++;
                            matchResult = offer.Matches(productCode, remainingQty);
                        } while (matchResult.IsMatch);

                        return (remainingQty, offers);
                    }
                    return acc;
                });

            var totalPrice = matchedOffers.Aggregate(0m, (sum, kvp) => sum + kvp.Key.OfferPrice * kvp.Value);
            if (remainingProductQty > 0)
            {
                totalPrice += await CalculateForProductWithoutOffersAsync(productCode, remainingProductQty).ConfigureAwait(false);
            }

            return totalPrice;
        }

        private async Task<decimal> CalculateForProductWithoutOffersAsync(string productCode, int productQty)
        {
            var productPrice = await GetProductPriceAsync(productCode).ConfigureAwait(false);
            return productPrice * productQty;
        }

        private async Task<decimal> GetProductPriceAsync(string productCode)
        {
            var product = await _productRepository.GetByCodeAsync(productCode).ConfigureAwait(false);
            return product?.UnitPrice ?? throw new ProductNotFoundException($"Product with code {productCode} was not found.");
        }
    }
}
