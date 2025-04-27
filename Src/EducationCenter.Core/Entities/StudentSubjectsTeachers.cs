

namespace EducationCenter.Core.Entities;


public class StudentSubjectsTeachers
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int SubjectTeacherId { get; set; }
    public SubjectTeacher SubjectTeacher { get; set; }
}
