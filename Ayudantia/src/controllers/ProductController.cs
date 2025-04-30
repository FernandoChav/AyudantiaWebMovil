using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Extensions;
using Ayudantia.Src.Helpers;
using Ayudantia.Src.Models;
using Ayudantia.Src.RequestHelpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ayudantia.Src.Controllers;


public class ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork) : BaseController
{
    private readonly ILogger<ProductController> _logger = logger;
    private readonly UnitOfWork _context = unitOfWork;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetPaged([FromQuery] ProductParams productParams)
    {
        var query = _context.ProductRepository.GetQueryableProducts();

        // Aplicar filtros y orden
        query = query.Search(productParams.Search)
                     .Filter(productParams.Brands, productParams.Categories)
                     .Sort(productParams.OrderBy);

        // Obtener paginación
        var pagedList = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

        // Agregar metadata de paginación al Header HTTP
        Response.AddPaginationHeader(pagedList.Metadata);

        // Retornar respuesta estándar
        var response = new ApiResponse<IEnumerable<Product>>(
            true,
            "Products retrieved successfully",
            pagedList
        );

        return Ok(response);
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