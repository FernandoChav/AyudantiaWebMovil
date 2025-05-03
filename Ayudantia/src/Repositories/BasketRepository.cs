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
    public class BasketRepository(StoreContext context) : IBasketRepository
    {
        private readonly StoreContext _context = context;

        public async Task<Basket?> GetBasketAsync(string basketId)
        {
            return await _context.Baskets
                .Include(x => x.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.BasketId == basketId);
        }

        public Basket CreateBasket(string basketId)
        {
            var basket = new Basket { BasketId = basketId };
            _context.Baskets.Add(basket);
            return basket;
        }

        public void UpdateBasket(Basket basket)
        {
            _context.Baskets.Update(basket);
        }

        public void DeleteBasket(Basket basket)
        {
            _context.Baskets.Remove(basket);
        }


        
    }
}