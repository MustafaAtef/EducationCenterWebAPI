using System;
using System.Linq.Expressions;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EducationCenterAPI.Services;

public class TeachersService : ITeachersService
{
    private readonly AppDbContext _appDbContext;
    public TeachersService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task CreateTeacherAsync(CreateTeacherDto createTeacherDto)
    {
        var existingTeacher = await _appDbContext.Teachers.SingleOrDefaultAsync(t => t.Email == createTeacherDto.Email);
        if (existingTeacher != null)
        {
            throw new UniqueException("Teacher already exists.");
        }
        var subjectsExist = await _appDbContext.Subjects.Where(s => createTeacherDto.Subjects.Contains(s.Id)).ToListAsync();
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
        _appDbContext.Teachers.Add(teacher);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<TeacherDto> GetTeacherByIdAsync(int id)
    {
        var teacher = await _appDbContext.Teachers.Where(t => t.Id == id).Select(t => new TeacherDto
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
        }).FirstOrDefaultAsync();

        if (teacher == null)
        {
            throw new BadRequestException("Teacher not found.");
        }
        return teacher;
    }

    public async Task<PagedList<TeacherSalaryDto>> GetTeacherSalariesAsync(int teacherId, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 12;
        var teacher = await _appDbContext.Teachers.SingleOrDefaultAsync(t => t.Id == teacherId);
        if (teacher == null)
        {
            throw new BadRequestException("Teacher not found.");
        }
        var teacherSalaries = _appDbContext.TeacherSalaries.Where(ts => ts.TeacherId == teacherId).Select(ts => new TeacherSalaryDto
        {
            Id = ts.ExpenseId,
            Name = ts.Teacher.Name,
            Months = ts.Months,
            Salary = ts.Salary,
            Paid = ts.Paid,
            Notes = ts.Notes,
            PaidAt = DateOnly.FromDateTime(ts.PaidAt),
        });
        return await PagedList<TeacherSalaryDto>.Create(teacherSalaries, page, pageSize);
    }

    public Task<PagedList<TeacherDto>> GetTeachersAsync(int page, int pageSize, string? searchTerm, string? sortBy, string? sortOrder, string? subject)
    {
        var query = _appDbContext.Teachers.AsQueryable();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(t => t.Name.Contains(searchTerm));
        }
        if (!string.IsNullOrEmpty(subject))
        {
            query = query.Where(t => t.Subjects.Any(s => s.Name == subject));
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
        if (sortOrder?.ToLower() == "asc")
        {
            query = query.OrderBy(keySelector);
        }
        else
        {
            query = query.OrderByDescending(keySelector);
        }

        return PagedList<TeacherDto>.Create(query.Select(t => new TeacherDto
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
        }), page, pageSize);
    }

    public Task<PagedList<TeacherSalaryDto>> GetTeachersSalariesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate)
    {
        var teacherSalariesQuery = _appDbContext.TeacherSalaries.AsQueryable();
        if (fromDate is not null && toDate is not null && DateOnly.TryParse(fromDate, out DateOnly parsedFromDate) && DateOnly.TryParse(toDate, out DateOnly parsedToDate))
        {
            teacherSalariesQuery = teacherSalariesQuery.Where(ts => ts.PaidAt >= parsedFromDate.ToDateTime(new TimeOnly(0, 0)) && ts.PaidAt <= parsedToDate.ToDateTime(new TimeOnly(23, 59)));
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
        if (sortOrder?.ToLower() == "asc")
        {
            teacherSalariesQuery = teacherSalariesQuery.OrderBy(keySelector);
        }
        else
        {
            teacherSalariesQuery = teacherSalariesQuery.OrderByDescending(keySelector);
        }
        return PagedList<TeacherSalaryDto>.Create(teacherSalariesQuery.Select(ts => new TeacherSalaryDto
        {
            Id = ts.ExpenseId,
            Name = ts.Teacher.Name,
            Months = ts.Months,
            Salary = ts.Salary,
            Paid = ts.Paid,
            Notes = ts.Notes,
            PaidAt = DateOnly.FromDateTime(ts.PaidAt),
        }), page, pageSize);
    }

    public async Task PayTeacherSalaryAsync(PayTeacherSalaryDto payTeacherSalaryDto)
    {
        var teacher = await _appDbContext.Teachers.SingleOrDefaultAsync(t => t.Id == payTeacherSalaryDto.TeacherId);
        if (teacher == null)
        {
            throw new BadRequestException("Teacher not found.");
        }
        var existingSalary = await _appDbContext.TeacherSalaries.SingleOrDefaultAsync(ts => ts.TeacherId == payTeacherSalaryDto.TeacherId && ts.Months == payTeacherSalaryDto.Months);
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
        _appDbContext.Expenses.Add(expense);
        _appDbContext.TeacherSalaries.Add(salary);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateTeacherAsync(UpdateTeacherDto updateTeacherDto)
    {
        var existingTeacher = await _appDbContext.Teachers.SingleOrDefaultAsync(t => t.Id == updateTeacherDto.Id);
        if (existingTeacher == null)
        {
            throw new BadRequestException("Invalid teacher id provided.");
        }
        var subjectsExist = await _appDbContext.Subjects.Where(s => updateTeacherDto.Subjects.Contains(s.Id)).ToListAsync();
        if (subjectsExist.Count != updateTeacherDto.Subjects.Count)
        {
            throw new BadRequestException("Invalid subject id(s) provided.");
        }
        await _appDbContext.SubjectsTeachers.Where(st => st.TeachersId == existingTeacher.Id).ExecuteDeleteAsync();
        existingTeacher.Name = updateTeacherDto.Name;
        existingTeacher.Email = updateTeacherDto.Email;
        existingTeacher.Phone = updateTeacherDto.Phone;
        existingTeacher.Subjects = subjectsExist;
        _appDbContext.Teachers.Update(existingTeacher);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateTeacherSalaryAsync(UpdateTeacherSalaryDto updateTeacherSalaryDto)
    {
        var teacherSalary = await _appDbContext.TeacherSalaries.SingleOrDefaultAsync(ts => ts.ExpenseId == updateTeacherSalaryDto.Id);
        if (teacherSalary == null)
        {
            throw new BadRequestException("Invalid teacher salary id provided.");
        }
        if (teacherSalary.Months != updateTeacherSalaryDto.Months)
        {
            var updatedMonths = await _appDbContext.TeacherSalaries.SingleOrDefaultAsync(ts => ts.Months == updateTeacherSalaryDto.Months && ts.TeacherId == teacherSalary.TeacherId);
            if (updatedMonths != null)
            {
                throw new UniqueException("Salary already paid for this month.");
            }
        }
        teacherSalary.Months = updateTeacherSalaryDto.Months.Value;
        teacherSalary.Salary = updateTeacherSalaryDto.Amount.Value;
        teacherSalary.Paid = updateTeacherSalaryDto.Paid.Value;
        teacherSalary.Notes = updateTeacherSalaryDto.Notes;
        _appDbContext.TeacherSalaries.Update(teacherSalary);
        await _appDbContext.SaveChangesAsync();
    }
}
