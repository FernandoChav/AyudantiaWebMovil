using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Interfaces;
using Ayudantia.Src.Models;

using Microsoft.EntityFrameworkCore;

namespace Ayudantia.Src.Repositories
{
    public class ShippingAddressRepository(StoreContext context) : IShippingAddressRepository
    {
        private readonly StoreContext _context = context;

        public async Task<ShippingAddres?> GetByUserIdAsync(string userId)
        {
            return await _context.ShippingAddres
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task AddAsync(ShippingAddres address)
        {
            await _context.ShippingAddres.AddAsync(address);
        }
    }

}