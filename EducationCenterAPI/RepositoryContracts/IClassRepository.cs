using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.RepositoryContracts;

public interface IClassRepository : IRepository<Class>
{
    Task<IEnumerable<ClassStudentDto>> GetClassStudentsWithAttendanceStatusAsync(int classId);
    Task<IEnumerable<ClassStudentDto>> GetTodayClassesStudentsWithAttendanceStatusAsync();
    Task<IList<AttendanceStatisticsDto>> GetClassAttendanceStats(int classId);
}