using System;

namespace EducationCenter.Core.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Subject> Subjects { get; set; }
}
