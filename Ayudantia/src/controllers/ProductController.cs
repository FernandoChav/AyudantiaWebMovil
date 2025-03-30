using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.src.data;
using Ayudantia.src.models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ayudantia.src.controllers
{
    [Route("[controller]")] //localhost:5000/Product
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly StoreContext _context;
        public ProductController(ILogger<ProductController> logger, StoreContext context)
        {
            _context = context;
            _logger = logger;
        }
       

        [HttpGet]
        public ActionResult<List<Product>> GetAll()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }
    }
}