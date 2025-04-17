using System.ComponentModel.DataAnnotations;

namespace EducationCenterAPI.Dtos;
public class CreateGradeDto
{
    [StringLength(100, ErrorMessage = "Grade name's length must be less than 100 charachters")]
    public string Name { get; set; }
}

public class GradeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}