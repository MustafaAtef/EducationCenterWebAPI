using System;

namespace EducationCenter.Core.Entities;


public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int GradeId { get; set; }
    public Grade Grade { get; set; }
    public ICollection<Teacher> Teachers { get; set; }
}
