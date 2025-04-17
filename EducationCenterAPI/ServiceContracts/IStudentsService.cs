using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface IStudentsService
{
    Task CreateStudentAsync(CreateStudentDto createStudentDto);
    Task UpdateStudentAsync(UpdateStudentDto updateStudentDto);
    Task<PagedList<StudentDto>> GetAllStudentsAsync(int page, int pageSize, string? searchItem, string? sortBy, string? sortOrder, int? gradeId);
    Task<StudentDto> GetStudentByIdAsync(int id);
}
