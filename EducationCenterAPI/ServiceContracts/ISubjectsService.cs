using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface ISubjectsService
{
    Task CreateSubjectAsync(CreateSubjectDto createSubjectDto);
    Task UpdateSubjectAsync(UpdateSubjectDto updateSubjectDto);
    Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync(int? gradeId);

    Task<IEnumerable<SubjectWithTeacherDto>> GetSubjectsWithTeachersByGradeIdAsync(int id);
}
