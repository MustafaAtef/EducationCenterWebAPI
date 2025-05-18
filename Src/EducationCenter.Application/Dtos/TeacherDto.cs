using System.ComponentModel.DataAnnotations;
using EducationCenter.Application.CustomValidations;

namespace EducationCenter.Application.Dtos;

public class CreateTeacherDto
{
    [StringLength(100, ErrorMessage = "Teacher name maximum length is 100 characters")]
    [Required(ErrorMessage = "Teacher name is required")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "Teacher email maximum length is 100 characters")]
    [EmailAddress(ErrorMessage = "Invalid provided email")]
    [Required(ErrorMessage = "Teacher email is required")]
    public string Email { get; set; }

    [StringLength(11, ErrorMessage = "Teacher phone maximum length is 11 numbers")]
    [PhoneNumber(ErrorMessage = "Invalid provided phone number")]
    [Required(ErrorMessage = "Teacher phone is required")]
    public string Phone { get; set; }

    public ICollection<int> Subjects { get; set; }

}

public class UpdateTeacherDto
{
    public int Id { get; set; }
    [StringLength(100, ErrorMessage = "Teacher name maximum length is 100 characters")]
    [Required(ErrorMessage = "Teacher name is required")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "Teacher email maximum length is 100 characters")]
    [EmailAddress(ErrorMessage = "Invalid provided email")]
    [Required(ErrorMessage = "Teacher email is required")]
    public string Email { get; set; }

    [StringLength(11, ErrorMessage = "Teacher phone maximum length is 11 numbers")]
    [PhoneNumber(ErrorMessage = "Invalid provided phone number")]
    [Required(ErrorMessage = "Teacher phone is required")]
    public string Phone { get; set; }
    public ICollection<int> Subjects { get; set; }
}

public class TeacherDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public ICollection<SubjectDto> Teaches { get; set; }
    public DateTime CreatedAt { get; set; }
}