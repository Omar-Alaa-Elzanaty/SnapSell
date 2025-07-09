using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSell.Application.Extensions.Services
{
    public static class EnumerableExtension
    {
        public static bool IsEmptyOrNull<T>(this IEnumerable<T>? source) => source != null && source.Any();
    }
}
