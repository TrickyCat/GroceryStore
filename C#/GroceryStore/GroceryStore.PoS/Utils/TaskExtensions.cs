using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.PoS.Utils
{
    public static class TaskExtensions
    {
        public static Task<TResult[]> SelectAndWaitAll<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selector) 
            => source.Select(selector).WhenAll();

        

        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks) 
            => tasks != null ? Task.WhenAll(tasks) : Task.FromResult<T[]>(null);
    }
}
