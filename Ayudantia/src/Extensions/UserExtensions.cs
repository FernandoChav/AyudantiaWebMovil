using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Models;

namespace Ayudantia.Src.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> Filter(this IQueryable<User> query, bool? isActive, DateTime? from, DateTime? to)
        {
            if (isActive.HasValue)
                query = query.Where(u => u.IsActive == isActive.Value);

            if (from.HasValue)
                query = query.Where(u => u.RegisteredAt >= from.Value);

            if (to.HasValue)
                query = query.Where(u => u.RegisteredAt <= to.Value);

            return query;
        }

        public static IQueryable<User> Search(this IQueryable<User> query, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return query;

            var lower = search.Trim().ToLower();

            return query.Where(u =>
                u.FirtsName.ToLower().Contains(lower) ||
                u.LastName.ToLower().Contains(lower) ||
                (u.Email != null && u.Email.ToLower().Contains(lower))
            );
        }

        public static IQueryable<User> Sort(this IQueryable<User> query, string? orderBy)
        {
            return orderBy switch
            {
                "name" => query.OrderBy(u => u.FirtsName),
                "nameDesc" => query.OrderByDescending(u => u.FirtsName),
                "date" => query.OrderBy(u => u.RegisteredAt),
                "dateDesc" => query.OrderByDescending(u => u.RegisteredAt),
                _ => query.OrderByDescending(u => u.RegisteredAt)
            };
        }
    }
}