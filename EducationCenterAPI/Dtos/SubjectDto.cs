using System;
using System.ComponentModel.DataAnnotations;

namespace EducationCenterAPI.Dtos;

public class CreateSubjectDto
{
    [StringLength(100, ErrorMessage = "Subject name length must be less than 100 characters")]
    public string Name { get; set; }
    public ICollection<int> Grades { get; set; }
}

public class UpdateSubjectDto
{
    public string OldName { get; set; }
    [StringLength(100, ErrorMessage = "Subject name length must be less than 100 characters")]
    public string Name { get; set; }
    public ICollection<int> Grades { get; set; }
}

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public string Grade { get; set; } = string.Empty;
}