using System.Collections.Generic;
using System.Linq;

namespace Blazor.Performance.Client.Utils
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> Add<T>(this IEnumerable<T> enumerable, T value)
        {
            return enumerable.Concat(new T[] { value });
        }
        
        public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            int current = 0;
            foreach (var item in enumerable)
            {
                if (current == index)
                    yield return value;

                yield return item;
                current++;
            }
        }
        
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            int current = 0;
            foreach (var item in enumerable)
            {
                yield return current == index ? value : item;
                current++;
            }
        }
        
        public static IEnumerable<T> Remove<T>(this IEnumerable<T> enumerable, int index)
        {
            return enumerable.Where((x, i) => index != i);
        }
    }
}