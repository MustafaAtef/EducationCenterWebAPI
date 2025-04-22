using System.Linq.Expressions;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.RepositoryContracts;
using EducationCenterAPI.ServiceContracts;

namespace EducationCenterAPI.Services;

public class SubjectsService : ISubjectsService
{
    private readonly IUnitOfWork _unitOfWork;

    public SubjectsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateSubjectAsync(CreateSubjectDto createSubjectDto)
    {
        var gradesExist = await _unitOfWork.Grades.CountAsync(g => createSubjectDto.Grades.Contains(g.Id));
        var subjectsAlreadyExist = await _unitOfWork.Subjects.CountAsync(s => s.Name == createSubjectDto.Name && createSubjectDto.Grades.Contains(s.GradeId));
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
            _unitOfWork.Subjects.Add(new()
            {
                GradeId = gradeId,
                Name = createSubjectDto.Name,
            });
        }
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync(int? gradeId)
    {
        Expression<Func<Subject, bool>>? predicate = null;
        if (gradeId is not null)
        {
            predicate = sub => sub.GradeId == gradeId;
        }
        var subjects = await _unitOfWork.Subjects.FindAllAsync(predicate, new string[] { "Grade" });
        return subjects.Select(s => new SubjectDto
        {
            Id = s.Id,
            Name = s.Name,
            GradeId = s.GradeId,
            Grade = s.Grade.Name,
        });
    }

    public async Task<IEnumerable<SubjectWithTeacherDto>> GetSubjectsWithTeachersByGradeIdAsync(int id)
    {
        return (await _unitOfWork.SubjectsTeachers.FindAllAsync(st => st.Subject.GradeId == id, new string[] { "Subject", "Teacher" })).Select(st => new SubjectWithTeacherDto
        {
            Id = st.Id,
            Name = st.Subject.Name,
            Teacher = st.Teacher.Name,
        });
    }

    public async Task UpdateSubjectAsync(UpdateSubjectDto updateSubjectDto)
    {
        var gradesExist = await _unitOfWork.Grades.CountAsync(g => updateSubjectDto.Grades.Contains(g.Id));
        if (gradesExist != updateSubjectDto.Grades.Count)
        {
            throw new BadRequestException("Invalid grade id(s) provided.");
        }
        if (updateSubjectDto.OldName != updateSubjectDto.Name)
        {
            var subjectsAlreadyExist = await _unitOfWork.Subjects.CountAsync(s => s.Name == updateSubjectDto.Name && updateSubjectDto.Grades.Contains(s.GradeId));
            if (subjectsAlreadyExist > 0)
            {
                throw new UniqueException("Subject already exists.");
            }
        }
        await _unitOfWork.Subjects.DeleteSubjectByName(updateSubjectDto.OldName);
        foreach (var gradeId in updateSubjectDto.Grades)
        {
            _unitOfWork.Subjects.Add(new()
            {
                GradeId = gradeId,
                Name = updateSubjectDto.Name,
            });
        }
        await _unitOfWork.SaveChangesAsync();
    }
}
