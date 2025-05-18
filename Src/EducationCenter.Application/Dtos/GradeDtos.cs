using System.ComponentModel.DataAnnotations;

namespace EducationCenter.Application.Dtos;

public class CreateGradeDto
{
    [StringLength(100, ErrorMessage = "Grade name's length must be less than 100 charachters")]
    [Required(ErrorMessage = "Grade name is required")]
    public string Name { get; set; }
}

public class GradeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}