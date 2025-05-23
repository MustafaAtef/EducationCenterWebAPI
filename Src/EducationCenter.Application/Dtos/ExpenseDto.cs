using System;
using System.ComponentModel.DataAnnotations;

namespace EducationCenter.Application.Dtos;


public class PayStudentFeesDto : IValidatableObject
{

    public int StudentId { get; set; }
    [Required]
    public DateOnly? Months { get; set; }
    [Required(ErrorMessage = "Fees amount is required")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Paid amount is required")]
    public decimal? Paid { get; set; }
    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount is not null && Paid is not null && Amount < Paid)
        {
            yield return new ValidationResult("Paid amount should be less than or equal to fees amount", new[] { nameof(Paid) });
        }
    }
}

public class UpdateStudentFeesDto : IValidatableObject
{

    public int Id { get; set; }

    public int StudentId { get; set; }
    [Required]
    public DateOnly? Months { get; set; }
    [Required(ErrorMessage = "Fees amount is required")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Paid amount is required")]
    public decimal? Paid { get; set; }
    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount is not null && Paid is not null && Amount < Paid)
        {
            yield return new ValidationResult("Paid amount should be less than or equal to fees amount", new[] { nameof(Paid) });
        }
    }
}

public class StudentFeeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Grade { get; set; }
    public DateOnly Months { get; set; }
    public decimal Amount { get; set; }
    public decimal Paid { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime PaidAt { get; set; }
}



public class PayTeacherSalaryDto : IValidatableObject
{

    public int TeacherId { get; set; }
    [Required]
    public DateOnly? Months { get; set; }
    [Required(ErrorMessage = "Fees amount is required")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Paid amount is required")]
    public decimal? Paid { get; set; }
    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount is not null && Paid is not null && Amount < Paid)
        {
            yield return new ValidationResult("Paid amount should be less than or equal to fees amount", new[] { nameof(Paid) });
        }
    }
}

public class UpdateTeacherSalaryDto : IValidatableObject
{

    public int Id { get; set; }

    public int TeacherId { get; set; }
    [Required]
    public DateOnly? Months { get; set; }
    [Required(ErrorMessage = "Fees amount is required")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Paid amount is required")]
    public decimal? Paid { get; set; }
    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount is not null && Paid is not null && Amount < Paid)
        {
            yield return new ValidationResult("Paid amount should be less than or equal to fees amount", new[] { nameof(Paid) });
        }
    }
}

public class TeacherSalaryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly Months { get; set; }
    public decimal Salary { get; set; }
    public decimal Paid { get; set; }
    public string? Notes { get; set; }
    public DateOnly PaidAt { get; set; }
}


public class PayOtherExpenseDto
{
    [Required(ErrorMessage = "Expense amount is required")]
    public decimal Amount { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class UpdateOtherExpenseDto
{

    public int Id { get; set; }
    [Required(ErrorMessage = "Expense amount is required")]
    public decimal Amount { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class OtherExpenseDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public DateTime PaidAt { get; set; }
}