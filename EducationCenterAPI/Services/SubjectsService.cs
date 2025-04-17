using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Services;

public class SubjectsService : ISubjectsService
{
    private readonly AppDbContext _appDbContext;

    public SubjectsService(AppDbContext appDbContext)
    {
        this._appDbContext = appDbContext;
    }
    public async Task CreateSubjectAsync(CreateSubjectDto createSubjectDto)
    {
        var gradesExist = await _appDbContext.Grades.CountAsync(g => createSubjectDto.Grades.Contains(g.Id));
        var subjectsAlreadyExist = await _appDbContext.Subjects.CountAsync(s => s.Name == createSubjectDto.Name && createSubjectDto.Grades.Contains(s.GradeId));
        if (subjectsAlreadyExist > 0)
        {
            throw new UniqueException("Subject already exists.");
        }
        if (gradesExist != createSubjectDto.Grades.Count)
        {
            throw new BadRequestException("Invalid grade id(s) provided.");
        }
        foreach (var gradeId in createSubjectDto.Grades)
        {
            _appDbContext.Subjects.Add(new()
            {
                GradeId = gradeId,
                Name = createSubjectDto.Name,
            });
        }
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync(int? gradeId)
    {
        var subjectsQuery = _appDbContext.Subjects.AsQueryable();
        if (gradeId is not null)
        {
            subjectsQuery = subjectsQuery.Where(sub => sub.GradeId == gradeId);
        }
        return await subjectsQuery.OrderBy(s => s.Name).ThenBy(s => s.Grade.Name).Select(s => new SubjectDto
        {
            Id = s.Id,
            Name = s.Name,
            GradeId = s.GradeId,
            Grade = s.Grade.Name,
        }).ToListAsync();
    }

    public async Task<IEnumerable<SubjectWithTeacherDto>> GetSubjectsWithTeachersByGradeIdAsync(int id)
    {
        return await _appDbContext.SubjectsTeachers.Where(st => st.Subject.GradeId == id).Select(st => new SubjectWithTeacherDto
        {
            Id = st.Id,
            Name = st.Subject.Name,
            Teacher = st.Teacher.Name,
        }).ToListAsync();
    }

    public async Task UpdateSubjectAsync(UpdateSubjectDto updateSubjectDto)
    {
        var gradesExist = await _appDbContext.Grades.CountAsync(g => updateSubjectDto.Grades.Contains(g.Id));
        if (gradesExist != updateSubjectDto.Grades.Count)
        {
            throw new BadRequestException("Invalid grade id(s) provided.");
        }
        if (updateSubjectDto.OldName != updateSubjectDto.Name)
        {
            var subjectsAlreadyExist = await _appDbContext.Subjects.CountAsync(s => s.Name == updateSubjectDto.Name && updateSubjectDto.Grades.Contains(s.GradeId));
            if (subjectsAlreadyExist > 0)
            {
                throw new UniqueException("Subject already exists.");
            }
        }
        await _appDbContext.Subjects.Where(sub => sub.Name == updateSubjectDto.OldName).ExecuteDeleteAsync();
        foreach (var gradeId in updateSubjectDto.Grades)
        {
            _appDbContext.Subjects.Add(new()
            {
                GradeId = gradeId,
                Name = updateSubjectDto.Name,
            });
        }
        await _appDbContext.SaveChangesAsync();
    }
}
