using System;
using System.Threading.Tasks;
using GroceryStore.PoS.Runner.Utils.DI;

namespace GroceryStore.PoS.Runner
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var container = CompositionRoot.Configure(new DiContainer());

            Console.WriteLine("Hello, YouScan!\n");

            await SimpleTestRunner.RunTests(container);

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }
    }
}
