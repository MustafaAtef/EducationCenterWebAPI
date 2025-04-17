using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using EducationCenterAPI.CustomValidations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EducationCenterAPI.Dtos;

public class CreateTeacherDto
{
    [StringLength(100, ErrorMessage = "Teacher name maximum length is 100 characters")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "Teacher email maximum length is 100 characters")]
    [EmailAddress(ErrorMessage = "Invalid provided email")]
    public string Email { get; set; }

    [StringLength(11, ErrorMessage = "Teacher phone maximum length is 11 numbers")]
    [PhoneNumber(ErrorMessage = "Invalid provided phone number")]
    public string Phone { get; set; }

    public ICollection<int> Subjects { get; set; }

}

public class UpdateTeacherDto
{
    [BindNever]
    public int Id { get; set; }
    [StringLength(100, ErrorMessage = "Teacher name maximum length is 100 characters")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "Teacher email maximum length is 100 characters")]
    [EmailAddress(ErrorMessage = "Invalid provided email")]
    public string Email { get; set; }

    [StringLength(11, ErrorMessage = "Teacher phone maximum length is 11 numbers")]
    [PhoneNumber(ErrorMessage = "Invalid provided phone number")]
    public string Phone { get; set; }
    public ICollection<int> Subjects { get; set; }
}


public class TeacherDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public ICollection<SubjectDto> Teaches { get; set; }
    public DateTime CreatedAt { get; set; }
}