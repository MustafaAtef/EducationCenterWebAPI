using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.RepositoryContracts;

namespace EducationCenterAPI.Repositories;

public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(AppDbContext context) : base(context)
    {
    }
}