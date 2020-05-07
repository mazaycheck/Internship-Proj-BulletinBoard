using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Helpers
{
    public class ValidDateOrNullAttribute : ValidationAttribute
    {


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) { return ValidationResult.Success; }
            DateTime date = (DateTime)value;
            if (date >= DateTime.Now.Date && date <= DateTime.Now.Date.AddDays(30))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Date must be equal or greater then {DateTime.Now.ToShortDateString()} and less then {DateTime.Now.AddDays(30).ToShortDateString()}");
            }
        }
    }
}
