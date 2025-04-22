using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.RepositoryContracts;

namespace EducationCenterAPI.Repositories;

public class StudentsRepository : Repository<Student>, IStudentsRepository
{
    public StudentsRepository(AppDbContext context) : base(context)
    {
    }
}