using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryStore.PoS.PointOfSale;
using GroceryStore.PoS.Runner.Utils.DI;

namespace GroceryStore.PoS.Runner
{
    class SimpleTestRunner
    {
        private static readonly List<TestInfo> TestInfos = new List<TestInfo>
        {
            new TestInfo
            {
                Title = "Test 1",
                Description = "Scan these items in this order: ABCDABA. Verify the total price is 13.25.",
                ExpectedTotal = 13.25m,
                ItemCodes = "ABCDABA".Select(c => c.ToString())
            },
            new TestInfo
            {
                Title = "Test 2",
                Description = "Scan these items in this order: CCCCCCC. Verify the total price is 6.00.",
                ExpectedTotal = 6m,
                ItemCodes = "CCCCCCC".Select(c => c.ToString())
            },
            new TestInfo
            {
                Title = "Test 3",
                Description = "Scan these items in this order: ABCD. Verify the total price is 7.25",
                ExpectedTotal = 7.25m,
                ItemCodes = "ABCD".Select(c => c.ToString())
            },


            new TestInfo
            {
                Title = "Test 4 (Test 1 with revoked codes)",
                Description = "Scan these items in this order: ABCDABAABCDABA. Then revoke these items: ABCDABA. Verify the total price is 13.25.",
                ExpectedTotal = 13.25m,
                ItemCodes = "ABCDABAABCDABA".Select(c => c.ToString()),
                RevokedItemCodes = "ABCDABA".Select(c => c.ToString())
            },
            new TestInfo
            {
                Title = "Test 5 (Test 2 with revoked codes)",
                Description = "Scan these items in this order: CCCCCCCCCCCCCC. Then revoke these items: CCCCCCC. Verify the total price is 6.00.",
                ExpectedTotal = 6m,
                ItemCodes = "CCCCCCCCCCCCCC".Select(c => c.ToString()),
                RevokedItemCodes = "CCCCCCC".Select(c => c.ToString())
            },
            new TestInfo
            {
                Title = "Test 6 (Test 3 with revoked codes)",
                Description = "Scan these items in this order: ABCDABCD. Then revoke these items: ABCD. Verify the total price is 7.25",
                ExpectedTotal = 7.25m,
                ItemCodes = "ABCDABCD".Select(c => c.ToString()),
                RevokedItemCodes = "ABCD".Select(c => c.ToString())
            },

            new TestInfo
            {
                Title = "Test 7",
                Description = "Scan these items in this order: AAAAAA. Verify the total price is 6",
                ExpectedTotal = 6m,
                ItemCodes = "AAAAAA".Select(c => c.ToString())
            },
        };

        public static async Task RunTests(IDiContainer container)
        {
            var allTestsPass = false;
            foreach (var testInfo in TestInfos)
            {
                allTestsPass = await Test(container, testInfo);
                Console.WriteLine();
                if (!allTestsPass)
                {
                    break;
                }
            }

            Console.WriteLine($"All tests pass: {allTestsPass}\n");
        }


        private sealed class TestInfo
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public IEnumerable<string> ItemCodes { get; set; }
            public IEnumerable<string> RevokedItemCodes { get; set; } = Enumerable.Empty<string>();
            public decimal ExpectedTotal { get; set; }
        }

        private static async Task<bool> Test(IDiContainer container, TestInfo info)
        {
            Console.WriteLine($"-------------------------- {info.Title} BEGIN ------------------------------");
            Console.WriteLine();
            Console.WriteLine(info.Description);

            var posTerminal = container.Resolve<IPosTerminal>();

            foreach (var itemCode in info.ItemCodes)
            {
                posTerminal.ScanItemCode(itemCode);
            }

            foreach (var revokedCode in info.RevokedItemCodes)
            {
                posTerminal.RemoveItemCode(revokedCode);
            }

            var actualTotal = await posTerminal.CalculateTotalAsync().ConfigureAwait(false);
            var passes = info.ExpectedTotal == actualTotal;
            
            Console.WriteLine();
            Console.WriteLine(posTerminal);
            Console.WriteLine($"\nExpected: {info.ExpectedTotal}\nActual:   {actualTotal}\nPasses:   {passes}");
            Console.WriteLine();
            Console.WriteLine($"-------------------------- {info.Title} END ------------------------------");
            return passes;
        }
    }
}
