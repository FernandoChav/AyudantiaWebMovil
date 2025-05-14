using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Interfaces;

namespace Ayudantia.Src.Data;

public class UnitOfWork(StoreContext context, IProductRepository productRepository, IUserRepository userRepository, IBasketRepository basketRepository, IOrderRepository orderRepository, IShippingAddressRepository ShippingAddressRepository)
{
    private readonly StoreContext _context = context;
    public IUserRepository UserRepository { get; set; } = userRepository;

    public IProductRepository ProductRepository { get; set; } = productRepository;
    public IBasketRepository BasketRepository { get; set; } = basketRepository;
    public IOrderRepository OrderRepository { get; set; } = orderRepository;
    public IShippingAddressRepository ShippingAddressRepository { get; set; } = ShippingAddressRepository;
    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }

}