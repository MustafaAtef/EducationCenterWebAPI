using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface ITeachersService
{
    Task CreateTeacherAsync(CreateTeacherDto createTeacherDto);
    Task UpdateTeacherAsync(UpdateTeacherDto updateTeacherDto);
    Task<PagedList<TeacherDto>> GetTeachersAsync(int page, int pageSize, string? searchTerm, string? sortBy, string? sortOrder, string? subject);
    Task<TeacherDto> GetTeacherByIdAsync(int id);
}
