using System;
using System.Linq.Expressions;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;

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
}
