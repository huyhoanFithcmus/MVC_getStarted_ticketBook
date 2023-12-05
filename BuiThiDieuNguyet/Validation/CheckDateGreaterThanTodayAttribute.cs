using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuiThiDieuNguyet.Validation
{
    public class CheckDateGreaterThanTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext
        validationContext)
        {
            DateTime dt = (DateTime)value;
            if (dt >= DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage ?? "Dữ liệu ngày phải lớn hơn ngày hôm nay");
        }
    }
}