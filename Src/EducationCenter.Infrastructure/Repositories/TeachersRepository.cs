using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;

namespace EducationCenterAPI.Repositories;

public class TeachersRepository : Repository<Teacher>, ITeachersRepository
{
    public TeachersRepository(AppDbContext context) : base(context)
    {
    }
}