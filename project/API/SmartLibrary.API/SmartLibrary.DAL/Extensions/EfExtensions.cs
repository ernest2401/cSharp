﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.DAL.Extensions
{
    public static class EfExtensions
    {
        public static Task<List<TSource>> ToListAsyncSafe<TSource>(
          this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }
    }
}
