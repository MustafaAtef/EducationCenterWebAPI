using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Repositories;

public class SubjectsRepository : Repository<Subject>, ISubjectsRepository
{
    public SubjectsRepository(AppDbContext context) : base(context)
    {
    }

    public async Task DeleteSubjectByName(string name)
    {
        await _dbContext.Set<Subject>().Where(s => s.Name == name).ExecuteDeleteAsync();
    }
}