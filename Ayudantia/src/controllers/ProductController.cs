using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Extensions;
using Ayudantia.Src.Helpers;
using Ayudantia.Src.Interfaces;
using Ayudantia.Src.Mappers;
using Ayudantia.Src.Models;
using Ayudantia.Src.RequestHelpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ayudantia.Src.Controllers;

public class ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork, IPhotoService photoService) : BaseController
{
    private readonly ILogger<ProductController> _logger = logger;
    private readonly UnitOfWork _context = unitOfWork;
    private readonly IPhotoService _photoService = photoService;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetPaged([FromQuery] ProductParams productParams)
    {
        var query = _context.ProductRepository.GetQueryableProducts();

        query = query.Search(productParams.Search)
                     .Filter(productParams.Brands, productParams.Categories)
                     .Sort(productParams.OrderBy);

        var pagedList = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

        Response.AddPaginationHeader(pagedList.Metadata);

        var response = new ApiResponse<IEnumerable<Product>>(
            true,
            "Products retrieved successfully",
            pagedList
        );

        return Ok(new ApiResponse<IEnumerable<Product>>(
            true,
            "Products retrieved successfully",
            pagedList
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<Product>>> GetById(int id)
    {
        var product = await _context.ProductRepository.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound(new ApiResponse<Product>(false, "Product not found"));
        }

        return Ok(new ApiResponse<Product>(true, "Product retrieved successfully", product));
    }

    [HttpPost("create")]
    public async Task<ActionResult<ApiResponse<Product>>> Create([FromForm] ProductDto dto)
    {
        var urls = new List<string>();
        string? publicId = null;

        foreach (var image in dto.Images)
        {
            var result = await _photoService.AddPhotoAsync(image);
            if (result.Error != null)
            {
                return BadRequest(new ApiResponse<Product>(
                    false,
                    "Error uploading image",
                    null,
                    new List<string> { result.Error.Message }
                ));
            }

            urls.Add(result.SecureUrl.AbsoluteUri);
            publicId ??= result.PublicId;
        }

        var product = ProductMapper.FromCreateDto(dto, urls, publicId);

        await _context.ProductRepository.AddProductAsync(product);
        await _context.SaveChangeAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            new ApiResponse<Product>(true, "Product created successfully", product)
        );
    }
}
