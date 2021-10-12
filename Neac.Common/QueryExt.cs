using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common
{
    public static class QueryExt
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            return (condition) ? source.Where(predicate) : source;
        }
        public static List<TSource> WhereIf<TSource>(this List<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            return (condition) ? source.Where(predicate).ToList() : source;
        }
    }
}
