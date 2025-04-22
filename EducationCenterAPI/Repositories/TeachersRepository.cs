using System.Linq.Expressions;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Repositories;

public class TeachersRepository : Repository<Teacher>, ITeachersRepository
{
    public TeachersRepository(AppDbContext context) : base(context)
    {
    }
}