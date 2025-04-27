namespace EducationCenter.Core.Entities;

public class Grade
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Student> Students { get; set; }
    public ICollection<Subject> Subjects { get; set; }
}

