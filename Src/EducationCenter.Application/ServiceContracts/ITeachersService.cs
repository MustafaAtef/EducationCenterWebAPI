using System;
using EducationCenter.Application.Dtos;

namespace EducationCenter.Application.ServiceContracts;

public interface ITeachersService
{
    Task CreateTeacherAsync(CreateTeacherDto createTeacherDto);
    Task UpdateTeacherAsync(UpdateTeacherDto updateTeacherDto);
    Task<PagedList<TeacherDto>> GetTeachersAsync(int page, int pageSize, string? searchTerm, string? sortBy, string? sortOrder, string? subject);
    Task<TeacherDto> GetTeacherByIdAsync(int id);
    Task PayTeacherSalaryAsync(PayTeacherSalaryDto payTeacherSalaryDto);
    Task UpdateTeacherSalaryAsync(UpdateTeacherSalaryDto updateTeacherSalaryDto);
    Task<PagedList<TeacherSalaryDto>> GetTeachersSalariesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate);
    Task<PagedList<TeacherSalaryDto>> GetTeacherSalariesAsync(int teacherId, int page, int pageSize);
}
