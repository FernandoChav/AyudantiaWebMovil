using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Models;

namespace Ayudantia.Src.Dtos
{
    public class ProductDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La categoría no puede exceder los 100 caracteres")]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "La marca no puede exceder los 100 caracteres")]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [Range(0, 100000, ErrorMessage = "El stock debe ser 0 o más.")]
        public int Stock { get; set; }

        [Required]

        public List<IFormFile> Images { get; set; } = [];
        public ProductCondition Condition { get; set; } = ProductCondition.Nuevo;
    }
}