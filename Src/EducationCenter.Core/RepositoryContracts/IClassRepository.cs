using EducationCenter.Core.Entities;

namespace EducationCenter.Core.RepositoryContracts;

public interface IClassRepository : IRepository<Class>
{
    Task<IEnumerable<ClassStudent>> GetClassStudentsWithAttendanceStatusAsync(int classId);
    Task<IEnumerable<ClassStudent>> GetTodayClassesStudentsWithAttendanceStatusAsync();
    Task<IList<AttendanceStatistics>> GetClassAttendanceStats(int classId);
}