using BuiThiDieuNguyet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuiThiDieuNguyet.Validation
{
    public class GenreAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext
        validationContext)
        {
            return ValidationResult.Success;
        }
    }
}