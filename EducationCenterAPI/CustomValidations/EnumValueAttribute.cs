using System;
using System.ComponentModel.DataAnnotations;

namespace EducationCenterAPI.CustomValidations;

public class EnumValueAttribute : ValidationAttribute
{

    private Type _enumType;
    public EnumValueAttribute(Type enumType)
    {
        _enumType = enumType;
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not null)
        {
            if (Enum.IsDefined(_enumType, value))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage ?? $"Invlaid {validationContext.MemberName} field value");
        }
        return null;
    }

}
