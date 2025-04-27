using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;

namespace EducationCenterAPI.Repositories;

public class TeacherSalariesRepository : Repository<TeacherSalary>, ITeacherSalariesRepository
{
    public TeacherSalariesRepository(AppDbContext appDbContext) : base(appDbContext)
    {

    }

}
