using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.RepositoryContracts;

namespace EducationCenterAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    public UnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    private IUserRepository? _users = null;
    private IClassRepository? _classes = null;
    private IAttendanceRepository? _attendances = null;
    private IExpenseRepository? _expenses = null;
    private IGradesRepository? _grades = null;
    private ISubjectsRepository? _subjects = null;
    private IStudentsRepository? _students = null;
    private ITeachersRepository? _teachers = null;
    private ITeacherSalariesRepository? _teacherSalaries = null;
    private ISubjectsTeachersRepository? _subjectsTeachers = null;
    private IStudentSubjectsTeachersRepository? _studentSubjectsTeachers = null;
    private IStudentFeesRepository? _studentFees = null;
    private IOtherExpensesRepository? _otherExpenses = null;

    public IUserRepository Users
    {
        get
        {
            if (_users == null)
                _users = new UserRepository(_appDbContext);
            return _users;
        }
    }

    public IClassRepository Classes
    {
        get
        {
            if (_classes == null)
                _classes = new ClassRepository(_appDbContext);
            return _classes;
        }
    }

    public IAttendanceRepository Attendances
    {
        get
        {
            if (_attendances == null)
                _attendances = new AttendanceRepository(_appDbContext);
            return _attendances;
        }
    }

    public IExpenseRepository Expenses
    {
        get
        {
            if (_expenses == null)
                _expenses = new ExpenseRepository(_appDbContext);
            return _expenses;
        }
    }

    public IGradesRepository Grades
    {
        get
        {
            if (_grades == null)
                _grades = new GradesRepository(_appDbContext);
            return _grades;
        }
    }

    public ISubjectsRepository Subjects
    {
        get
        {
            if (_subjects == null)
                _subjects = new SubjectsRepository(_appDbContext);
            return _subjects;
        }
    }

    public IStudentsRepository Students
    {
        get
        {
            if (_students == null)
                _students = new StudentsRepository(_appDbContext);
            return _students;
        }
    }

    public ITeachersRepository Teachers
    {
        get
        {
            if (_teachers == null)
                _teachers = new TeachersRepository(_appDbContext);
            return _teachers;
        }
    }

    public ITeacherSalariesRepository TeacherSalaries
    {
        get
        {
            if (_teacherSalaries == null)
                _teacherSalaries = new TeacherSalariesRepository(_appDbContext);
            return _teacherSalaries;
        }
    }

    public ISubjectsTeachersRepository SubjectsTeachers
    {
        get
        {
            if (_subjectsTeachers == null)
                _subjectsTeachers = new SubjectsTeachersRepository(_appDbContext);
            return _subjectsTeachers;
        }
    }

    public IStudentSubjectsTeachersRepository StudentSubjectsTeachers
    {
        get
        {
            if (_studentSubjectsTeachers == null)
                _studentSubjectsTeachers = new StudentSubjectsTeachersRepository(_appDbContext);
            return _studentSubjectsTeachers;
        }
    }

    public IStudentFeesRepository StudentFees
    {
        get
        {
            if (_studentFees == null)
                _studentFees = new StudentFeesRepository(_appDbContext);
            return _studentFees;
        }
    }

    public IOtherExpensesRepository OtherExpenses
    {
        get
        {
            if (_otherExpenses == null)
                _otherExpenses = new OtherExpensesRepository(_appDbContext);
            return _otherExpenses;
        }
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
    }

    public Task SaveChangesAsync()
    {
        return _appDbContext.SaveChangesAsync();
    }
}
