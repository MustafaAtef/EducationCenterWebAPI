using System;

namespace EducationCenterAPI.Database.Entities;

public class Class
{
    public int Id { get; set; }
    public int SubjectTeacherId { get; set; }
    public SubjectTeacher SubjectTeacher { get; set; }
    public DateOnly Date { get; set; }
    public string FromTime { get; set; }
    public string Totime { get; set; }
    public AttendanceRegistered AttendanceRegistered { get; set; }

}

public enum AttendanceRegistered
{
    NotRegistered = 0,
    Registered = 1,
    Upcoming = 2
}
