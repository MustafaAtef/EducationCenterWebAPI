using System;
using EducationCenter.Core.Entities;
using EducationCenter.Application.Dtos;

namespace EducationCenter.Application.ServiceContracts;

public interface IAttendanceService
{
    Task<IEnumerable<ClassStudent>> GetAttendanceAsync(int classId);
    Task<IEnumerable<ClassStudent>> GetTodayAttendanceAsync();
    Task RegisterStudentAttendanceAsync(RegisterStudentAttendanceDto takeStudentAttendanceDto);

}
