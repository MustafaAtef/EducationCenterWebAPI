using System;
using System.ComponentModel.DataAnnotations;

namespace EducationCenter.Application.Dtos;

public class CreateSubjectDto
{
    [StringLength(100, ErrorMessage = "Subject name length must be less than 100 characters")]
    [Required(ErrorMessage = "Subject name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Subject grades are required")]
    public ICollection<int> Grades { get; set; }
}

public class UpdateSubjectDto
{
    [Required(ErrorMessage = "Subject old name is required")]
    public string OldName { get; set; }
    [StringLength(100, ErrorMessage = "Subject name length must be less than 100 characters")]
    [Required(ErrorMessage = "Subject new name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Subject grades are required")]
    public ICollection<int> Grades { get; set; }
}

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public string Grade { get; set; } = string.Empty;
}