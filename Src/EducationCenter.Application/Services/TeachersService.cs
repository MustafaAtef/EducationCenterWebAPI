using System.Linq.Expressions;
using EducationCenter.Core;
using EducationCenter.Core.Entities;
using EducationCenter.Core.Exceptions;
using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using EducationCenter.Core.RepositoryContracts;

namespace EducationCenter.Application.Services;

public class TeachersService : ITeachersService
{
    private readonly IUnitOfWork _unitOfWork;
    public TeachersService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateTeacherAsync(CreateTeacherDto createTeacherDto)
    {
        var existingTeacher = await _unitOfWork.Teachers.FindAsync(t => t.Email == createTeacherDto.Email);
        if (existingTeacher != null)
        {
            throw new UniqueException("Teacher already exists.");
        }
        var subjectsExist = await _unitOfWork.Subjects.FindAllAsync(s => createTeacherDto.Subjects.Contains(s.Id));
        if (subjectsExist.Count != createTeacherDto.Subjects.Count)
        {
            throw new BadRequestException("Invalid subject id(s) provided.");
        }
        var teacher = new Teacher
        {
            Name = createTeacherDto.Name,
            Email = createTeacherDto.Email,
            Phone = createTeacherDto.Phone,
            Subjects = subjectsExist,
        };
        _unitOfWork.Teachers.Add(teacher);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<TeacherDto> GetTeacherByIdAsync(int id)
    {
        var teacher = await _unitOfWork.Teachers.FindAsync(t => t.Id == id, new string[] { "Subjects.Grade" });
        if (teacher == null)
        {
            throw new BadRequestException("Teacher not found.");
        }
        return new TeacherDto
        {
            Id = teacher.Id,
            Name = teacher.Name,
            Email = teacher.Email,
            Phone = teacher.Phone,
            CreatedAt = teacher.CreatedAt,
            Teaches = teacher.Subjects.Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                GradeId = s.Grade.Id,
                Grade = s.Grade.Name
            }).ToList()
        };
    }

    public async Task<PagedList<TeacherSalaryDto>> GetTeacherSalariesAsync(int teacherId, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 12;
        var teacher = await _unitOfWork.Teachers.FindAsync(t => t.Id == teacherId);
        if (teacher == null)
        {
            throw new BadRequestException("Teacher not found.");
        }
        var teacherSalaries = await _unitOfWork.TeacherSalaries.FindAllAsync(page, pageSize, ts => ts.TeacherId == teacherId, null, null, new string[] { "Teacher" });
        var totalTeacherSalaries = await _unitOfWork.TeacherSalaries.CountAsync(ts => ts.TeacherId == teacherId);
        return new PagedList<TeacherSalaryDto>(totalTeacherSalaries, pageSize, page, teacherSalaries.Select(ts => new TeacherSalaryDto
        {
            Id = ts.ExpenseId,
            Name = ts.Teacher.Name,
            Months = ts.Months,
            Salary = ts.Salary,
            Paid = ts.Paid,
            Notes = ts.Notes,
            PaidAt = DateOnly.FromDateTime(ts.PaidAt),
        }));
    }

    public async Task<PagedList<TeacherDto>> GetTeachersAsync(int page, int pageSize, string? searchTerm, string? sortBy, string? sortOrder, string? subject)
    {
        Expression<Func<Teacher, bool>>? predicate = null;

        if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(subject))
        {
            predicate = t => t.Name.Contains(searchTerm) && t.Subjects.Any(s => s.Name == subject);
        }
        else if (!string.IsNullOrEmpty(searchTerm))
        {
            predicate = t => t.Name.Contains(searchTerm);
        }
        else if (!string.IsNullOrEmpty(subject))
        {
            predicate = t => t.Subjects.Any(s => s.Name == subject);
        }

        Expression<Func<Teacher, object>> keySelector;
        switch (sortBy?.ToLower())
        {
            case "name":
                keySelector = t => t.Name;
                break;
            case "createdat":
                keySelector = t => t.CreatedAt;
                break;
            default:
                keySelector = t => t.CreatedAt;
                break;
        }

        var teachers = await _unitOfWork.Teachers.FindAllAsync(page, pageSize, predicate, keySelector, sortOrder, new string[] { "Subjects.Grade" });
        var totalTeachers = await _unitOfWork.Teachers.CountAsync(predicate);
        return new PagedList<TeacherDto>(totalTeachers, pageSize, page, teachers.Select(t => new TeacherDto
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Phone = t.Phone,
            CreatedAt = t.CreatedAt,
            Teaches = t.Subjects.Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name,
                GradeId = s.Grade.Id,
                Grade = s.Grade.Name
            }).ToList()
        }));
    }

    public async Task<PagedList<TeacherSalaryDto>> GetTeachersSalariesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate)
    {
        Expression<Func<TeacherSalary, bool>>? predicate = null;
        if (fromDate is not null && toDate is not null && DateOnly.TryParse(fromDate, out DateOnly parsedFromDate) && DateOnly.TryParse(toDate, out DateOnly parsedToDate))
        {
            predicate = ts => ts.PaidAt >= parsedFromDate.ToDateTime(new TimeOnly(0, 0)) && ts.PaidAt <= parsedToDate.ToDateTime(new TimeOnly(23, 59));
        }
        Expression<Func<TeacherSalary, object>> keySelector;
        switch (sortBy?.ToLower())
        {
            case "salary":
                keySelector = ts => ts.Salary;
                break;
            case "paidat":
                keySelector = ts => ts.PaidAt;
                break;
            default:
                keySelector = ts => ts.PaidAt;
                break;
        }
        var teachersSalaries = await _unitOfWork.TeacherSalaries.FindAllAsync(page, pageSize, predicate, keySelector, sortOrder, new string[] { "Teacher" });
        var totalTeachersSalaries = await _unitOfWork.TeacherSalaries.CountAsync(predicate);
        return new PagedList<TeacherSalaryDto>(totalTeachersSalaries, pageSize, page, teachersSalaries.Select(ts => new TeacherSalaryDto
        {
            Id = ts.ExpenseId,
            Name = ts.Teacher.Name,
            Months = ts.Months,
            Salary = ts.Salary,
            Paid = ts.Paid,
            Notes = ts.Notes,
            PaidAt = DateOnly.FromDateTime(ts.PaidAt),
        }));
    }

    public async Task PayTeacherSalaryAsync(PayTeacherSalaryDto payTeacherSalaryDto)
    {
        var teacher = await _unitOfWork.Teachers.FindAsync(t => t.Id == payTeacherSalaryDto.TeacherId);
        if (teacher == null)
        {
            throw new BadRequestException("Teacher not found.");
        }
        var existingSalary = await _unitOfWork.TeacherSalaries.FindAsync(ts => ts.TeacherId == payTeacherSalaryDto.TeacherId && ts.Months == payTeacherSalaryDto.Months);
        if (existingSalary != null)
        {
            throw new UniqueException("Salary already paid for this month.");
        }
        var expense = new Expense()
        {
            Paid = payTeacherSalaryDto.Paid.Value,
            ExpenseTypeId = (int)ExpenseTypeEnum.TeacherSalary
        };
        var salary = new TeacherSalary
        {
            Expense = expense,
            Teacher = teacher,
            Salary = payTeacherSalaryDto.Amount.Value,
            Months = payTeacherSalaryDto.Months.Value,
            Notes = payTeacherSalaryDto.Notes,
            Paid = payTeacherSalaryDto.Paid.Value,
        };
        _unitOfWork.Expenses.Add(expense);
        _unitOfWork.TeacherSalaries.Add(salary);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateTeacherAsync(UpdateTeacherDto updateTeacherDto)
    {
        var existingTeacher = await _unitOfWork.Teachers.FindAsync(t => t.Id == updateTeacherDto.Id);
        if (existingTeacher == null)
        {
            throw new BadRequestException("Invalid teacher id provided.");
        }
        var subjectsExist = await _unitOfWork.Subjects.FindAllAsync(s => updateTeacherDto.Subjects.Contains(s.Id));
        if (subjectsExist.Count != updateTeacherDto.Subjects.Count)
        {
            throw new BadRequestException("Invalid subject id(s) provided.");
        }
        await _unitOfWork.SubjectsTeachers.DeleteTeacherSubjets(existingTeacher.Id);
        existingTeacher.Name = updateTeacherDto.Name;
        existingTeacher.Email = updateTeacherDto.Email;
        existingTeacher.Phone = updateTeacherDto.Phone;
        existingTeacher.Subjects = subjectsExist;
        _unitOfWork.Teachers.Update(existingTeacher);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateTeacherSalaryAsync(UpdateTeacherSalaryDto updateTeacherSalaryDto)
    {
        var teacherSalary = await _unitOfWork.TeacherSalaries.FindAsync(ts => ts.ExpenseId == updateTeacherSalaryDto.Id);
        if (teacherSalary == null)
        {
            throw new BadRequestException("Invalid teacher salary id provided.");
        }
        if (teacherSalary.Months != updateTeacherSalaryDto.Months)
        {
            var updatedMonths = await _unitOfWork.TeacherSalaries.FindAsync(ts => ts.Months == updateTeacherSalaryDto.Months && ts.TeacherId == teacherSalary.TeacherId);
            if (updatedMonths != null)
            {
                throw new UniqueException("Salary already paid for this month.");
            }
        }
        teacherSalary.Months = updateTeacherSalaryDto.Months.Value;
        teacherSalary.Salary = updateTeacherSalaryDto.Amount.Value;
        teacherSalary.Paid = updateTeacherSalaryDto.Paid.Value;
        teacherSalary.Notes = updateTeacherSalaryDto.Notes;
        _unitOfWork.TeacherSalaries.Update(teacherSalary);
        await _unitOfWork.SaveChangesAsync();
    }
}
