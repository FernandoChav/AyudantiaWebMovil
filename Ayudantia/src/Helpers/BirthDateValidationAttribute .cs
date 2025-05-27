using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Helpers
{
    public class BirthDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateOnly birthDate)
                return ValidationResult.Success; // Si es nulo y no requerido, se permite

            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;

            if (birthDate >= today)
                return new ValidationResult("La fecha de nacimiento debe ser anterior a hoy.");

            if (age < 13)
                return new ValidationResult("Debes tener al menos 13 años para registrarte.");

            if (age > 100)
                return new ValidationResult("La edad máxima permitida es de 100 años.");

            return ValidationResult.Success;
        }
    }
}