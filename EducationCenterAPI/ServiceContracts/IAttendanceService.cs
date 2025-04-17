using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface IAttendanceService
{
    Task<IEnumerable<ClassStudentDto>> GetAttendanceAsync(int classId);
    Task<IEnumerable<ClassStudentDto>> GetTodayAttendanceAsync();
    Task RegisterStudentAttendanceAsync(RegisterStudentAttendanceAsyncDto takeStudentAttendanceDto);

}
