using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;

namespace EducationCenterAPI.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

}
