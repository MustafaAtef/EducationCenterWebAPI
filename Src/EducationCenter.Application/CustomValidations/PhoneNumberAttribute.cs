using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EducationCenter.Application.CustomValidations;

public class PhoneNumberAttribute : ValidationAttribute
{

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string pattern = @"^(011|012|010|015)\d{8}$";
        if (value is string phone && !Regex.IsMatch(phone, pattern))
        {
            return new ValidationResult(ErrorMessage ?? "Invalid phone number");
        }
        return ValidationResult.Success;
    }
}
