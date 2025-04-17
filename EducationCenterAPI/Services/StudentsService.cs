using System.Linq.Expressions;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Services;

public class StudentsService : IStudentsService
{
    private readonly AppDbContext _appDbContext;

    public StudentsService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<PagedList<StudentDto>> GetAllStudentsAsync(int page, int pageSize, string? searchItem, string? sortBy, string? sortOrder, int? gradeId)
    {
        var studentsQuery = _appDbContext.Students.AsQueryable();
        if (!string.IsNullOrEmpty(searchItem))
        {
            studentsQuery = studentsQuery.Where(s => s.Name.Contains(searchItem));
        }
        if (gradeId is not null)
        {
            studentsQuery = studentsQuery.Where(s => s.GradeId == gradeId);
        }
        Expression<Func<Student, object>> keySelector;
        switch (sortBy?.ToLower())
        {
            case "name":
                keySelector = s => s.Name;
                break;
            case "createdat":
                keySelector = s => s.CreatedAt;
                break;
            default:
                keySelector = s => s.CreatedAt;
                break;
        }
        if (sortOrder?.ToLower() == "asc")
        {
            studentsQuery = studentsQuery.OrderBy(keySelector);
        }
        else
        {
            studentsQuery = studentsQuery.OrderByDescending(keySelector);
        }

        return await PagedList<StudentDto>.Create(studentsQuery.Select(std =>
            new StudentDto
            {
                Id = std.Id,
                Name = std.Name,
                Email = std.Email,
                Phone = std.Phone,
                GradeId = std.GradeId,
                Grade = std.Grade.Name,
                CreatedAt = std.CreatedAt,
                Subjects = std.StudentSubjectsTeachers.Select(st => new SubjectWithTeacherDto
                {
                    Id = st.SubjectTeacher.Id,
                    Name = st.SubjectTeacher.Subject.Name,
                    Teacher = st.SubjectTeacher.Teacher.Name,
                }).ToList()
            }), page, pageSize);
    }

    public async Task<StudentDto> GetStudentByIdAsync(int id)
    {
        var student = await _appDbContext.Students.Where(std => std.Id == id).Select(std => new StudentDto()
        {
            Id = std.Id,
            Name = std.Name,
            Email = std.Email,
            Phone = std.Phone,
            GradeId = std.GradeId,
            Grade = std.Grade.Name,
            CreatedAt = std.CreatedAt,
            Subjects = std.StudentSubjectsTeachers.Select(st => new SubjectWithTeacherDto
            {
                Id = st.SubjectTeacher.Id,
                Name = st.SubjectTeacher.Subject.Name,
                Teacher = st.SubjectTeacher.Teacher.Name,
            }).ToList()
        }).FirstOrDefaultAsync();

        if (student is null)
        {
            throw new BadRequestException("Student does not exist");
        }
        return student;
    }

    public async Task CreateStudentAsync(CreateStudentDto createStudentDto)
    {
        var existingStudent = await _appDbContext.Students.SingleOrDefaultAsync(std => std.Email == createStudentDto.Email);
        if (existingStudent is not null)
        {
            throw new UniqueException("Email already exists");
        }
        var existingGrade = await _appDbContext.Grades.SingleOrDefaultAsync(g => g.Id == createStudentDto.GradeId);
        if (existingGrade is null)
        {
            throw new BadRequestException("Grade does not exist");
        }
        var subjectsTeachersExisted = await _appDbContext.SubjectsTeachers.Where(st => createStudentDto.SubjectIds.Contains(st.Id)).ToListAsync();
        if (subjectsTeachersExisted.Count != createStudentDto.SubjectIds.Count)
        {
            throw new BadRequestException("Some subjects do not exist");
        }
        var student = new Student
        {
            Name = createStudentDto.Name,
            Email = createStudentDto.Email,
            Phone = createStudentDto.Phone,
            Grade = existingGrade,
            StudentSubjectsTeachers = subjectsTeachersExisted.Select(st => new StudentSubjectsTeachers
            {
                SubjectTeacher = st
            }).ToList(),
        };
        _appDbContext.Students.Add(student);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateStudentAsync(UpdateStudentDto updateStudentDto)
    {
        var existedStudent = await _appDbContext.Students.SingleOrDefaultAsync(std => std.Id == updateStudentDto.Id);
        if (existedStudent is null)
        {
            throw new BadRequestException("Student does not exist");
        }
        var existedGrade = await _appDbContext.Grades.SingleOrDefaultAsync(g => g.Id == updateStudentDto.GradeId);
        if (existedGrade is null)
        {
            throw new BadRequestException("Grade does not exist");
        }
        var existedSubjectsTeachers = await _appDbContext.SubjectsTeachers.Where(st => updateStudentDto.SubjectIds.Contains(st.Id)).ToListAsync();
        if (existedSubjectsTeachers.Count != updateStudentDto.SubjectIds.Count)
        {
            throw new BadRequestException("Some subjects do not exist");
        }

        await _appDbContext.StudentSubjectsTeachers.Where(sst => sst.StudentId == updateStudentDto.Id).ExecuteDeleteAsync();

        existedStudent.Name = updateStudentDto.Name;
        existedStudent.Phone = updateStudentDto.Phone;
        existedStudent.Grade = existedGrade;
        existedStudent.StudentSubjectsTeachers = existedSubjectsTeachers.Select(st => new StudentSubjectsTeachers
        {
            SubjectTeacher = st
        }).ToList();

        _appDbContext.Students.Update(existedStudent);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task PayStudentFeesAsync(PayStudentFeesDto payStudentFeesDto)
    {
        var student = await _appDbContext.Students.SingleOrDefaultAsync(std => std.Id == payStudentFeesDto.StudentId);
        if (student is null)
        {
            throw new BadRequestException("Student does not exist");
        }
        var studentFees = await _appDbContext.StudentFees.SingleOrDefaultAsync(sf => sf.StudentId == payStudentFeesDto.StudentId && sf.Months == payStudentFeesDto.Months);
        if (studentFees is not null)
        {
            throw new BadRequestException("Student fees already paid for this month");
        }
        var expense = new Expense()
        {
            Paid = payStudentFeesDto.Paid.Value,
            ExpenseTypeId = (int)ExpenseTypeEnum.StudentFee,
        };
        var studentFee = new StudentFee()
        {
            Amount = payStudentFeesDto.Amount.Value,
            Paid = payStudentFeesDto.Paid.Value,
            Months = payStudentFeesDto.Months.Value,
            Notes = payStudentFeesDto.Notes,
            Student = student,
            Expense = expense,
        };
        _appDbContext.Expenses.Add(expense);
        _appDbContext.StudentFees.Add(studentFee);
        await _appDbContext.SaveChangesAsync();
    }

    public Task<PagedList<StudentFeeDto>> GetStudentsFeesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate)
    {
        var studentFeesQuery = _appDbContext.StudentFees.AsQueryable();
        if (fromDate is not null && toDate is not null && DateOnly.TryParse(fromDate, out DateOnly parsedFromDate) && DateOnly.TryParse(toDate, out DateOnly parsedToDate))
        {
            studentFeesQuery = studentFeesQuery.Where(sf => sf.PaidAt >= parsedFromDate.ToDateTime(new TimeOnly(0, 0)) && sf.PaidAt <= parsedToDate.ToDateTime(new TimeOnly(23, 59)));
        }
        Expression<Func<StudentFee, object>> keySelector;
        switch (sortBy?.ToLower())
        {
            case "amount":
                keySelector = sf => sf.Amount;
                break;
            case "paidat":
                keySelector = sf => sf.PaidAt;
                break;
            default:
                keySelector = sf => sf.PaidAt;
                break;
        }
        if (sortOrder?.ToLower() == "asc")
        {
            studentFeesQuery = studentFeesQuery.OrderBy(keySelector);
        }
        else
        {
            studentFeesQuery = studentFeesQuery.OrderByDescending(keySelector);
        }

        return PagedList<StudentFeeDto>.Create(studentFeesQuery.Select(sf => new StudentFeeDto()
        {
            Id = sf.ExpenseId,
            Name = sf.Student.Name,
            Grade = sf.Student.Grade.Name,
            Months = sf.Months,
            Amount = sf.Amount,
            Paid = sf.Paid,
            Notes = sf.Notes,
            PaidAt = sf.PaidAt,
        }), page, pageSize);
    }

    public async Task<PagedList<StudentFeeDto>> GetStudentFeesAsync(int studentId, int page, int pageSize)
    {
        if (page == 0) page = 1;
        if (pageSize == 0) pageSize = 12;
        var student = await _appDbContext.Students.SingleOrDefaultAsync(std => std.Id == studentId);
        if (student is null)
        {
            throw new BadRequestException("Student does not exist");
        }
        var studentFeesQuery = _appDbContext.StudentFees.Where(sf => sf.StudentId == studentId).Select(sf => new StudentFeeDto()
        {
            Id = sf.ExpenseId,
            Name = sf.Student.Name,
            Grade = sf.Student.Grade.Name,
            Months = sf.Months,
            Amount = sf.Amount,
            Paid = sf.Paid,
            Notes = sf.Notes,
            PaidAt = sf.PaidAt,
        }).OrderByDescending(sf => sf.PaidAt);
        return await PagedList<StudentFeeDto>.Create(studentFeesQuery, page, pageSize);
    }

    public async Task UpdateStudentFeesAsync(UpdateStudentFeesDto updateStudentFeesDto)
    {
        var studentFee = await _appDbContext.StudentFees.SingleOrDefaultAsync(sf => sf.ExpenseId == updateStudentFeesDto.Id);
        if (studentFee is null)
        {
            throw new BadRequestException("Student fee does not exist");
        }
        if (studentFee.Months != updateStudentFeesDto.Months)
        {
            var updatedMonths = await _appDbContext.StudentFees.SingleOrDefaultAsync(sf => sf.StudentId == studentFee.StudentId && sf.Months == updateStudentFeesDto.Months.Value);
            if (updatedMonths is not null)
            {
                throw new BadRequestException("Student fees already paid for this month");
            }
        }
        studentFee.Amount = updateStudentFeesDto.Amount.Value;
        studentFee.Paid = updateStudentFeesDto.Paid.Value;
        studentFee.Months = updateStudentFeesDto.Months.Value;
        studentFee.Notes = updateStudentFeesDto.Notes;
        _appDbContext.StudentFees.Update(studentFee);
        await _appDbContext.SaveChangesAsync();
    }
}
