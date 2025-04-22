using System;

namespace EducationCenterAPI.RepositoryContracts;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IClassRepository Classes { get; }
    IAttendanceRepository Attendances { get; }
    IExpenseRepository Expenses { get; }
    IGradesRepository Grades { get; }
    ISubjectsRepository Subjects { get; }
    IStudentsRepository Students { get; }
    ITeachersRepository Teachers { get; }
    ITeacherSalariesRepository TeacherSalaries { get; }
    ISubjectsTeachersRepository SubjectsTeachers { get; }
    IStudentSubjectsTeachersRepository StudentSubjectsTeachers { get; }
    IStudentFeesRepository StudentFees { get; }
    IOtherExpensesRepository OtherExpenses { get; }


    Task SaveChangesAsync();

}
