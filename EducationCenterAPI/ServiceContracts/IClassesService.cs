using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface IClassesService
{
    Task CreateClassAsync(CreateClassDto createClassDto);
    Task UpdateClassAsync(UpdateClassDto updateClassDto);
    Task<IEnumerable<ClassDto>> GetAllClassesAsync(int weekOffset, int? GradeId);
    Task<IEnumerable<ClassDto>> GetTodayClassesAsync();
    Task<ClassAttendanceStatisticsDto> GetClassAttendanceStatisticsAsync(int classId);
}
