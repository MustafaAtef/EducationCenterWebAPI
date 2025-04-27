using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;

namespace EducationCenterAPI.Repositories;

public class StudentFeesRepository : Repository<StudentFee>, IStudentFeesRepository
{
    public StudentFeesRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}
