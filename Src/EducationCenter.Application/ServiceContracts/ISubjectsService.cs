using System;
using EducationCenter.Application.Dtos;

namespace EducationCenter.Application.ServiceContracts;

public interface ISubjectsService
{
    Task CreateSubjectAsync(CreateSubjectDto createSubjectDto);
    Task UpdateSubjectAsync(UpdateSubjectDto updateSubjectDto);
    Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync(int? gradeId);

    Task<IEnumerable<SubjectWithTeacherDto>> GetSubjectsWithTeachersByGradeIdAsync(int id);
}
