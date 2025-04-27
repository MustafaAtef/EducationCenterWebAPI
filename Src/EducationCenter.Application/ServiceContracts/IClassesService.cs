using System;
using EducationCenter.Application.Dtos;

namespace EducationCenter.Application.ServiceContracts;

public interface IClassesService
{
    Task CreateClassAsync(CreateClassDto createClassDto);
    Task UpdateClassAsync(UpdateClassDto updateClassDto);
    Task<IEnumerable<ClassDto>> GetAllClassesAsync(int weekOffset, int? GradeId);
    Task<IEnumerable<ClassDto>> GetTodayClassesAsync();
    Task<ClassAttendanceStatisticsDto> GetClassAttendanceStatisticsAsync(int classId);
}
