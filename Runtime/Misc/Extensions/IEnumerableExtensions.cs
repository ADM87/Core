using System;
using System.Collections.Generic;

namespace ADM.Core
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<TElement>(this IEnumerable<TElement> elements, Action<TElement> operation)
        {
            foreach (var element in elements)
                operation(element);
        }
    }
}
