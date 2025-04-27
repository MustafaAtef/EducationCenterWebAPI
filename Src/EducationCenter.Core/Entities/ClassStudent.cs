using System;

namespace EducationCenter.Core.Entities;

public class ClassStudent
{
    public int ClassId { get; set; }
    public int StudentId { get; set; }
    public string Name { get; set; }
    public string Grade { get; set; }
    public string Subject { get; set; }
    public string Teacher { get; set; }
    public int IsPresent { get; set; }
}
