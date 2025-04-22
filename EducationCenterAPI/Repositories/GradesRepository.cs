using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.RepositoryContracts;

namespace EducationCenterAPI.Repositories;

public class GradesRepository : Repository<Grade>, IGradesRepository
{
    public GradesRepository(AppDbContext context) : base(context)
    {
    }
}