using System.Linq.Expressions;
using EducationCenter.Core;
using EducationCenter.Core.Entities;
using EducationCenter.Core.Exceptions;
using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using EducationCenter.Core.RepositoryContracts;

namespace EducationCenter.Application.Services;

public class StudentsService : IStudentsService
{
    private readonly IUnitOfWork _unitOfWork;


    public StudentsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<StudentDto>> GetAllStudentsAsync(int page, int pageSize, string? searchItem, string? sortBy, string? sortOrder, int? gradeId)
    {
        Expression<Func<Student, bool>>? predicate = null;

        if (!string.IsNullOrEmpty(searchItem) && gradeId is not null)
        {
            predicate = s => s.Name.Contains(searchItem) && s.GradeId == gradeId;
        }
        else if (!string.IsNullOrEmpty(searchItem))
        {
            predicate = s => s.Name.Contains(searchItem);
        }
        else if (gradeId is not null)
        {
            predicate = s => s.GradeId == gradeId;
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
        var students = await _unitOfWork.Students.FindAllAsync(page, pageSize, predicate, keySelector, sortOrder, new string[] { "Grade", "StudentSubjectsTeachers.SubjectTeacher.Subject", "StudentSubjectsTeachers.SubjectTeacher.Teacher" });
        var totalStudents = await _unitOfWork.Students.CountAsync(predicate);
        return new PagedList<StudentDto>(totalStudents, pageSize, page, students.Select(std =>
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
            }));
    }

    public async Task<StudentDto> GetStudentByIdAsync(int id)
    {
        var student = await _unitOfWork.Students.FindAsync(std => std.Id == id, new string[] { "Grade", "StudentSubjectsTeachers.Subject", "StudentSubjectsTeachers.Teacher" });
        if (student is null)
        {
            throw new BadRequestException("Student does not exist");
        }
        return new StudentDto()
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Phone = student.Phone,
            GradeId = student.GradeId,
            Grade = student.Grade.Name,
            CreatedAt = student.CreatedAt,
            Subjects = student.StudentSubjectsTeachers.Select(st => new SubjectWithTeacherDto
            {
                Id = st.SubjectTeacher.Id,
                Name = st.SubjectTeacher.Subject.Name,
                Teacher = st.SubjectTeacher.Teacher.Name,
            }).ToList()
        };
    }

    public async Task CreateStudentAsync(CreateStudentDto createStudentDto)
    {
        var existingStudent = await _unitOfWork.Students.FindAsync(std => std.Email == createStudentDto.Email);
        if (existingStudent is not null)
        {
            throw new UniqueException("Email already exists");
        }
        var existingGrade = await _unitOfWork.Grades.FindAsync(g => g.Id == createStudentDto.GradeId);
        if (existingGrade is null)
        {
            throw new BadRequestException("Grade does not exist");
        }
        var subjectsTeachersExisted = await _unitOfWork.SubjectsTeachers.FindAllAsync(st => createStudentDto.SubjectIds.Contains(st.Id));
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
        _unitOfWork.Students.Add(student);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateStudentAsync(UpdateStudentDto updateStudentDto)
    {
        var existedStudent = await _unitOfWork.Students.FindAsync(std => std.Id == updateStudentDto.Id);
        if (existedStudent is null)
        {
            throw new BadRequestException("Student does not exist");
        }
        var existedGrade = await _unitOfWork.Grades.FindAsync(g => g.Id == updateStudentDto.GradeId);
        if (existedGrade is null)
        {
            throw new BadRequestException("Grade does not exist");
        }
        var existedSubjectsTeachers = await _unitOfWork.SubjectsTeachers.FindAllAsync(st => updateStudentDto.SubjectIds.Contains(st.Id));
        if (existedSubjectsTeachers.Count != updateStudentDto.SubjectIds.Count)
        {
            throw new BadRequestException("Some subjects do not exist");
        }

        await _unitOfWork.StudentSubjectsTeachers.DeleteByStudentId(updateStudentDto.Id);

        existedStudent.Name = updateStudentDto.Name;
        existedStudent.Phone = updateStudentDto.Phone;
        existedStudent.Grade = existedGrade;
        existedStudent.StudentSubjectsTeachers = existedSubjectsTeachers.Select(st => new StudentSubjectsTeachers
        {
            SubjectTeacher = st
        }).ToList();

        _unitOfWork.Students.Update(existedStudent);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task PayStudentFeesAsync(PayStudentFeesDto payStudentFeesDto)
    {
        var student = await _unitOfWork.Students.FindAsync(std => std.Id == payStudentFeesDto.StudentId);
        if (student is null)
        {
            throw new BadRequestException("Student does not exist");
        }
        var studentFees = await _unitOfWork.StudentFees.FindAsync(sf => sf.StudentId == payStudentFeesDto.StudentId && sf.Months == payStudentFeesDto.Months);
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
        _unitOfWork.Expenses.Add(expense);
        _unitOfWork.StudentFees.Add(studentFee);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PagedList<StudentFeeDto>> GetStudentsFeesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate)
    {
        Expression<Func<StudentFee, bool>>? predicate = null;
        if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && DateOnly.TryParse(fromDate, out DateOnly parsedFromDate) && DateOnly.TryParse(toDate, out DateOnly parsedToDate))
        {
            predicate = sf => sf.PaidAt >= parsedFromDate.ToDateTime(new TimeOnly(0, 0)) && sf.PaidAt <= parsedToDate.ToDateTime(new TimeOnly(23, 59));
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
        var studentFees = await _unitOfWork.StudentFees.FindAllAsync(page, pageSize, predicate, keySelector, sortOrder, new string[] { "Student.Grade" });
        var totalStudentFees = await _unitOfWork.StudentFees.CountAsync(predicate);
        return new PagedList<StudentFeeDto>(totalStudentFees, pageSize, page, studentFees.Select(sf => new StudentFeeDto()
        {
            Id = sf.ExpenseId,
            Name = sf.Student.Name,
            Grade = sf.Student.Grade.Name,
            Months = sf.Months,
            Amount = sf.Amount,
            Paid = sf.Paid,
            Notes = sf.Notes,
            PaidAt = sf.PaidAt,
        }));
    }

    public async Task<PagedList<StudentFeeDto>> GetStudentFeesAsync(int studentId, int page, int pageSize)
    {
        if (page == 0) page = 1;
        if (pageSize == 0) pageSize = 12;
        var student = await _unitOfWork.Students.FindAsync(std => std.Id == studentId);
        if (student is null)
        {
            throw new BadRequestException("Student does not exist");
        }
        var studentFees = await _unitOfWork.StudentFees.FindAllAsync(page, pageSize, sf => sf.StudentId == studentId, sf => sf.PaidAt, "desc", new string[] { "Student.Grade" });
        var totalStudentFees = await _unitOfWork.StudentFees.CountAsync(sf => sf.StudentId == studentId);
        return new PagedList<StudentFeeDto>(totalStudentFees, pageSize, page, studentFees.Select(sf => new StudentFeeDto()
        {
            Id = sf.ExpenseId,
            Name = sf.Student.Name,
            Grade = sf.Student.Grade.Name,
            Months = sf.Months,
            Amount = sf.Amount,
            Paid = sf.Paid,
            Notes = sf.Notes,
            PaidAt = sf.PaidAt,
        }));
    }

    public async Task UpdateStudentFeesAsync(UpdateStudentFeesDto updateStudentFeesDto)
    {
        var studentFee = await _unitOfWork.StudentFees.FindAsync(sf => sf.ExpenseId == updateStudentFeesDto.Id);
        if (studentFee is null)
        {
            throw new BadRequestException("Student fee does not exist");
        }
        if (studentFee.Months != updateStudentFeesDto.Months)
        {
            var updatedMonths = await _unitOfWork.StudentFees.FindAsync(sf => sf.StudentId == studentFee.StudentId && sf.Months == updateStudentFeesDto.Months.Value);
            if (updatedMonths is not null)
            {
                throw new BadRequestException("Student fees already paid for this month");
            }
        }
        studentFee.Amount = updateStudentFeesDto.Amount.Value;
        studentFee.Paid = updateStudentFeesDto.Paid.Value;
        studentFee.Months = updateStudentFeesDto.Months.Value;
        studentFee.Notes = updateStudentFeesDto.Notes;
        _unitOfWork.StudentFees.Update(studentFee);
        await _unitOfWork.SaveChangesAsync();
    }
}
