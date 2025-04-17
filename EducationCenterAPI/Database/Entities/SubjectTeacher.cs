using System;

namespace EducationCenterAPI.Database.Entities;

public class SubjectTeacher
{
    public int Id { get; set; }
    public int TeachersId { get; set; }
    public Teacher Teacher { get; set; }
    public int SubjectsId { get; set; }
    public Subject Subject { get; set; }

}
