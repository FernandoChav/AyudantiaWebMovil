using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Models;

namespace Ayudantia.Src.Interfaces
{
    public interface IShippingAddressRepository
    {
        Task<ShippingAddres?> GetByUserIdAsync(string userId);
        Task AddAsync(ShippingAddres address);
    }
}