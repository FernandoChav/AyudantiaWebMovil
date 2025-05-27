using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos
{
    public class ToggleStatusDto
    {
        [StringLength(255)]
        [Required(ErrorMessage = "El motivo es obligatorio.")]
        public string Reason { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        public string Email { get; set; } = string.Empty;
    }
}