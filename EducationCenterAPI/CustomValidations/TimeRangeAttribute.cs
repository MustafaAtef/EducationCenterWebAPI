using System;
using System.ComponentModel.DataAnnotations;

namespace EducationCenterAPI.CustomValidations;

public class TimeRangeAttribute : ValidationAttribute
{
    private readonly string _startTimePropertyName;
    private readonly string _endTimePropertyName;

    public TimeRangeAttribute(string startTimePropertyName, string endTimePropertyName)
    {
        _startTimePropertyName = startTimePropertyName;
        _endTimePropertyName = endTimePropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var startTime = (string?)validationContext.ObjectType.GetProperty(_startTimePropertyName)?.GetValue(validationContext.ObjectInstance);
        var endTime = (string?)validationContext.ObjectType.GetProperty(_endTimePropertyName)?.GetValue(validationContext.ObjectInstance);

        if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
        {
            return new ValidationResult("Start time and end time are required.");
        }
        else
        {
            if (!isValidTime(startTime))
            {
                return new ValidationResult("Start time is not valid.");
            }

            if (!isValidTime(endTime))
            {
                return new ValidationResult("End time is not valid.");
            }

            if (string.Compare(startTime, endTime) >= 0)
            {
                return new ValidationResult("Start time must be less than end time.");
            }
        }
        return ValidationResult.Success;
    }

    private bool isValidTime(string time)
    {
        if (time.Length != 5) return false;
        if (time[2] != ':') return false;
        int h1 = time[0] - '0';
        int h2 = time[1] - '0';
        int m1 = time[3] - '0';
        int m2 = time[4] - '0';
        if (h1 < 0 || h1 > 2) return false;
        if (h1 == 2 && h2 > 3) return false;
        if (m1 < 0 || m1 > 5) return false;
        if (m2 < 0 || m2 > 9) return false;
        return true;
    }

}
