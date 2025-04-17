using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface IStudentsService
{
    Task CreateStudentAsync(CreateStudentDto createStudentDto);
    Task UpdateStudentAsync(UpdateStudentDto updateStudentDto);
    Task<PagedList<StudentDto>> GetAllStudentsAsync(int page, int pageSize, string? searchItem, string? sortBy, string? sortOrder, int? gradeId);
    Task<StudentDto> GetStudentByIdAsync(int id);
    Task PayStudentFeesAsync(PayStudentFeesDto payStudentFeesDto);
    Task UpdateStudentFeesAsync(UpdateStudentFeesDto updateStudentFeesDto);
    Task<PagedList<StudentFeeDto>> GetStudentsFeesAsync(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate);
    Task<PagedList<StudentFeeDto>> GetStudentFeesAsync(int studentId, int page, int pageSize);
}
