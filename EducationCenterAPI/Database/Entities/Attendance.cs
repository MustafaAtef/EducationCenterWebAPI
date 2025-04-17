using System;

namespace EducationCenterAPI.Database.Entities;

public class Attendance
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int ClassId { get; set; }
    public Class Class { get; set; }
    public bool IsPresent { get; set; }
    public DateTime RegisteredAt { get; set; }
}
