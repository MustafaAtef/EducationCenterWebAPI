using System.ComponentModel.DataAnnotations;
using EducationCenter.Application.CustomValidations;

namespace EducationCenter.Application.Dtos;

public class CreateClassDto : IValidatableObject
{
    [Required]
    public int? SubjectTeacherId { get; set; }

    [TimeRange(nameof(FromTime), nameof(ToTime))]
    public string FromTime { get; set; }
    public string ToTime { get; set; }
    public ICollection<int> Days { get; set; }

    [EnumValue(typeof(ClassRepeation))]
    public ClassRepeation Repeats { get; set; } = ClassRepeation.NoRepeat;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Days is not null)
        {
            var daysDict = new HashSet<int>();
            foreach (var day in Days)
            {
                if (day < 1 || day > 7)
                {
                    yield return new ValidationResult("Days must be between 1(Saturday) and 7(Friday)", new[] { nameof(Days) });
                }
                else if (!daysDict.Add(day))
                {
                    yield return new ValidationResult("Days must be unique", new[] { nameof(Days) });
                }
            }
        }
        else
        {
            yield return new ValidationResult("Days are required", new[] { nameof(Days) });
        }
    }
}

public class UpdateClassDto
{
    public int Id { get; set; }
    [Required]
    public int? SubjectTeacherId { get; set; }
    [TimeRange(nameof(FromTime), nameof(ToTime))]
    public string FromTime { get; set; }
    public string ToTime { get; set; }
}

public class ClassDto
{
    public int Id { get; set; }
    public int GradeId { get; set; }
    public string Grade { get; set; }
    public int SubjectTeacherId { get; set; }
    public string Subject { get; set; }
    public string Teacher { get; set; }
    public DateOnly Date { get; set; }
    public string FromTime { get; set; }
    public string ToTime { get; set; }
    public int AttendanceRegistered { get; set; }
}



public class ClassAttendanceStatisticsDto
{
    public int Id { get; set; }
    public int GradeId { get; set; }
    public string? Grade { get; set; }
    public int SubjectTeacherId { get; set; }
    public string? Subject { get; set; }
    public string? Teacher { get; set; }
    public DateOnly Date { get; set; }
    public string? FromTime { get; set; }
    public string? ToTime { get; set; }
    public int AttendanceRegistered { get; set; }
    public int TotalStudents { get; set; }
    public int Presents { get; set; }
    public int Absents { get; set; }
}
public enum ClassRepeation
{
    NoRepeat = 1,
    TwoWeeks = 2,
    OneMonths = 4,
    TwoMonths = 8,
    ThreeMonths = 12
}