using System;

namespace EducationCenterAPI.Database.Entities;

public class ExpenseType
{
    public int Id { get; set; }
    public string Name { get; set; }

}

public enum ExpenseTypeEnum
{
    StudentFee = 1,
    TeacherSalary = 2,
    OhterExpense = 3,
}
