using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;

namespace EducationCenterAPI.Repositories;

public class StudentsRepository : Repository<Student>, IStudentsRepository
{
    public StudentsRepository(AppDbContext context) : base(context)
    {
    }
}