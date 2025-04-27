using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Repositories;

public class ClassRepository : Repository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IList<AttendanceStatistics>> GetClassAttendanceStats(int classId)
    {
        return await _dbContext.Set<AttendanceStatistics>().FromSql($"EXEC GetClassAttendanceStats {classId}").ToListAsync();
    }

    public async Task<IEnumerable<ClassStudent>> GetClassStudentsWithAttendanceStatusAsync(int classId)
    {
        return await _dbContext.Set<ClassStudent>().FromSql($"EXEC GetClassStudentsWithAttendanceStatus {classId}").ToListAsync();
    }

    public async Task<IEnumerable<ClassStudent>> GetTodayClassesStudentsWithAttendanceStatusAsync()
    {
        return await _dbContext.Set<ClassStudent>().FromSql($"EXEC GetTodayClassesStudentsWithAttendanceStatus").ToListAsync();
    }
}