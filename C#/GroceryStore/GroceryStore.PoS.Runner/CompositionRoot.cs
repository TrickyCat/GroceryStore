using GroceryStore.PoS.Cart;
using GroceryStore.PoS.PointOfSale;
using GroceryStore.PoS.PriceCalculation;
using GroceryStore.PoS.Products;
using GroceryStore.PoS.Runner.Utils.DI;

namespace GroceryStore.PoS.Runner
{
    internal static class CompositionRoot
    {
        public static IDiContainer Configure(IDiContainer container)
        {
            container.Register<IPosTerminal, SimplePosTerminal>().LifeTimeMode = InstanceLifeTimeMode.NewInstance;
            container.Register<IPriceCalculator, SimplePriceCalculator>().LifeTimeMode = InstanceLifeTimeMode.NewInstance;
            container.Register<IShoppingCart, ShoppingCart>().LifeTimeMode = InstanceLifeTimeMode.NewInstance;
            container.Register<IProductRepository, FakeProductRepository>().LifeTimeMode = InstanceLifeTimeMode.NewInstance;
            container.Register<IProductOfferRepository, FakeProductOfferRepository>().LifeTimeMode = InstanceLifeTimeMode.NewInstance;

            return container;
        }
    }
}
