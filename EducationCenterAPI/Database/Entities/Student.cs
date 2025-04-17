using System;

namespace EducationCenterAPI.Database.Entities;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedAt { get; set; }
    public int GradeId { get; set; }
    public Grade Grade { get; set; }
    public ICollection<StudentSubjectsTeachers> StudentSubjectsTeachers { get; set; }
}
