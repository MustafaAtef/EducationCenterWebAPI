using System;

namespace EducationCenter.Core.Entities;


public class OtherExpense
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int ExpenseId { get; set; }
    public Expense Expense { get; set; } = null!;
    public DateTime PaidAt { get; set; }
}
