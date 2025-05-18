using System;
using System.ComponentModel.DataAnnotations;
using EducationCenter.Application.CustomValidations;

namespace EducationCenter.Application.Dtos;

public class CreateStudentDto
{
    [StringLength(100, ErrorMessage = "Student name maximum length is 100 characters")]
    [Required(ErrorMessage = "Student name is required")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "Student email maximum length is 100 characters")]
    [EmailAddress(ErrorMessage = "Invalid provided email")]
    [Required(ErrorMessage = "Student email is required")]
    public string Email { get; set; }

    [StringLength(11, ErrorMessage = "Student phone maximum length is 11 numbers")]
    [PhoneNumber(ErrorMessage = "Invalid provided phone number")]
    [Required(ErrorMessage = "Student phone is required")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Grade is required")]
    public int? GradeId { get; set; }
    [Required(ErrorMessage = "Student must be assigned to at least one subject")]
    public ICollection<int> SubjectIds { get; set; }

}

public class UpdateStudentDto
{

    public int Id { get; set; }
    [Required(ErrorMessage = "Student name is required")]
    [StringLength(100, ErrorMessage = "Student name maximum length is 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Student email is required")]
    [StringLength(100, ErrorMessage = "Student email maximum length is 100 characters")]
    [EmailAddress(ErrorMessage = "Invalid provided email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Student phone is required")]
    [StringLength(11, ErrorMessage = "Student phone maximum length is 11 numbers")]
    [PhoneNumber(ErrorMessage = "Invalid provided phone number")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Grade is required")]
    public int? GradeId { get; set; }
    [Required(ErrorMessage = "Student must be assigned to at least one subject")]
    public ICollection<int> SubjectIds { get; set; }
}

public class RegisterStudentAttendanceDto
{

    public int ClassId { get; set; }
    [Required]
    public int? StudentId { get; set; }
    [Required]
    public int? IsPresent { get; set; }
}

public class StudentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public string Grade { get; set; } = string.Empty;
    public ICollection<SubjectWithTeacherDto> Subjects { get; set; }
    public DateTime CreatedAt { get; set; }
}
