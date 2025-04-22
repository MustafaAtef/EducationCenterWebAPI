using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Repositories;

public class ClassRepository : Repository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IList<AttendanceStatisticsDto>> GetClassAttendanceStats(int classId)
    {
        return await _dbContext.Set<AttendanceStatisticsDto>().FromSql($"EXEC GetClassAttendanceStats {classId}").ToListAsync();
    }

    public async Task<IEnumerable<ClassStudentDto>> GetClassStudentsWithAttendanceStatusAsync(int classId)
    {
        return await _dbContext.Set<ClassStudentDto>().FromSql($"EXEC GetClassStudentsWithAttendanceStatus {classId}").ToListAsync();
    }

    public async Task<IEnumerable<ClassStudentDto>> GetTodayClassesStudentsWithAttendanceStatusAsync()
    {
        return await _dbContext.Set<ClassStudentDto>().FromSql($"EXEC GetTodayClassesStudentsWithAttendanceStatus").ToListAsync();
    }
}