using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Models;
using Ayudantia.Src.RequestHelpers;

namespace Ayudantia.Src.Extensions
{
    public static class OrderExtensions
    {
        public static IQueryable<Order> Filter(this IQueryable<Order> query, OrderParams param)
        {
            if (param.FromDate.HasValue)
            {
                var from = param.FromDate.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(o => o.OrderDate >= from);
            }

            if (param.ToDate.HasValue)
            {
                var to = param.ToDate.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(o => o.OrderDate <= to);
            }

            if (param.MinTotal.HasValue)
            {
                query = query.Where(o => o.Total >= param.MinTotal.Value);
            }

            if (param.MaxTotal.HasValue)
            {
                query = query.Where(o => o.Total <= param.MaxTotal.Value);
            }

            return query.OrderByDescending(o => o.OrderDate);
        }
    }
}