using System.Threading.Tasks;

namespace GroceryStore.PoS.PointOfSale
{
    public interface IPosTerminal
    {
        void ScanItemCode(string code);
        void RemoveItemCode(string code); // aka Revoke product from cart
        Task<decimal> CalculateTotalAsync();
    }
}
