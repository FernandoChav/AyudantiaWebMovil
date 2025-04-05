using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Models;

using Microsoft.EntityFrameworkCore;

namespace Ayudantia.Src.Data;

public class StoreContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Product> Products { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<ShippingAddres> ShippingAddres { get; set; }
}