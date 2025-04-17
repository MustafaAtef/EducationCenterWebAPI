using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface IAttendanceService
{
    Task<IEnumerable<ClassStudentDto>> GetAttendance(int classId);
}
