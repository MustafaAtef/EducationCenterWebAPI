using System;

namespace EducationCenterAPI.Database.Entities;

public class TeacherSalary
{
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public DateOnly Months { get; set; }
    public decimal Salary { get; set; }
    public decimal Paid { get; set; }
    public int ExpenseId { get; set; }
    public Expense Expense { get; set; }
    public string Notes { get; set; }
    public DateTime PaidAt { get; set; }

}
