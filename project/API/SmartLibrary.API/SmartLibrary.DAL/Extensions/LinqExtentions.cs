using System;
using System.Linq;
using System.Linq.Expressions;

namespace SmartLibrary.DAL.Extensions
{
    /// <summary>
    /// Represents class for linq extentions
    /// </summary>
    public static class LinqExtentions
    {
        /// <summary>
        /// Applies predicate if condition is true
        /// </summary>
        /// <param name="source">The collection of <see cref="{TSource}"/> items</param>
        /// <param name="condition">Bool condition</param>
        /// <param name="predicate">The instance of <see cref="Expression{Func{TSource, bool}}"/> delegate</param>
        /// <returns>The collection of <see cref="{TSource}"/> items</returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }

            return source;
        }
    }
}
