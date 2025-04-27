using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;

namespace EducationCenterAPI.Repositories;

public class GradesRepository : Repository<Grade>, IGradesRepository
{
    public GradesRepository(AppDbContext context) : base(context)
    {
    }
}