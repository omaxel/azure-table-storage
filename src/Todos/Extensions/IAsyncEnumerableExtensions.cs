using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todos.Extensions
{
    public static class IAsyncEnumerableExtensions
    {
        public static async Task<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetAsyncEnumerator();
            await enumerator.MoveNextAsync();
            return enumerator.Current;
        }
    }
}
