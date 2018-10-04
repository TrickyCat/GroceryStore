using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.PoS.Products
{
    public class FakeProductRepository: IProductRepository
    {
        private static readonly List<Product> Products = new List<Product>
        {
            new Product("A", "Avocado",    1.25m),
            new Product("B", "Beetroot",   4.25m),
            new Product("C", "Carrot",     1m),
            new Product("D", "Durian",     0.75m),

            new Product("M", "Melon",      2.15m),
            new Product("W", "Watermelon", 1.65m),
            new Product("MNG", "Mango",    2.95m),
            new Product("G", "Guava",      1.95m),
            new Product("P", "Pineapple",  3.95m)
        };


        public Task<Product> GetByCodeAsync(string productCode)
        {
            var product = Products.FirstOrDefault(p =>
                string.Equals(p.Code, productCode, StringComparison.InvariantCultureIgnoreCase));
            return Task.FromResult(product);
        }
    }
}
