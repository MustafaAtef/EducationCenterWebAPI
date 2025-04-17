using System;

namespace EducationCenterAPI.Database.Entities;

public class StudentFee
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public DateOnly Months { get; set; }
    public decimal Amount { get; set; }
    public decimal Paid { get; set; }
    public int ExpenseId { get; set; }
    public Expense Expense { get; set; }
    public string Notes { get; set; }
    public DateTime PaidAt { get; set; }
}
