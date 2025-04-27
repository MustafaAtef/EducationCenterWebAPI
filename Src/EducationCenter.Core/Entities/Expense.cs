using System;

namespace EducationCenter.Core.Entities;


public class Expense
{
    public int Id { get; set; }
    public decimal Paid { get; set; }
    public int ExpenseTypeId { get; set; }
    public ExpenseType ExpenseType { get; set; }
    public DateTime PaidAt { get; set; }

}
