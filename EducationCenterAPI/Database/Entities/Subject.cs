using System;

namespace EducationCenterAPI.Database.Entities;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int GradeId { get; set; }
    public Grade Grade { get; set; }
    public ICollection<Teacher> Teachers { get; set; }
}
