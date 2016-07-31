using System.Collections.Generic;
using System.Linq;

namespace CSProjToXProj.Plumbing
{
    public static class EnumerableExtensions
    {
        public static bool None<T>(this IEnumerable<T> items)
        {
            return !items.Any();
        }
    }
}