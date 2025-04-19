using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ayudantia.Src.Controllers;


public class ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork) : BaseController
{
    private readonly ILogger<ProductController> _logger = logger;
    private readonly UnitOfWork _context = unitOfWork;

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {

        var products = await _context.ProductRepository.GetProductsAsync();
        return Ok(products);

    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _context.ProductRepository.GetProductByIdAsync(id);
        return product == null ? (ActionResult<Product>)NotFound() : (ActionResult<Product>)Ok(product);
    }
    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        await _context.ProductRepository.AddProductAsync(product);
        await _context.SaveChangeAsync();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}