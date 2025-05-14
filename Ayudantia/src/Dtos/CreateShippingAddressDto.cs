using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos
{
    public class CreateShippingAddressDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public required string Street { get; set; }
        [Required(ErrorMessage = "El número es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El número debe ser un valor numérico")]
        public required string Number { get; set; }
        [Required(ErrorMessage = "La comuna es requerida")]
        [StringLength(100, ErrorMessage = "La comuna no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "La comuna solo puede contener letras y espacios")]
        public required string Commune { get; set; }
        [Required(ErrorMessage = "La región es requerida")]
        [StringLength(100, ErrorMessage = "La región no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "La región solo puede contener letras y espacios")]
        public required string Region { get; set; }
        [Required(ErrorMessage = "El código postal es requerido")]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "El código postal debe tener 7 dígitos")]
        public required string PostalCode { get; set; }
    }
}