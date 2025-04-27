using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Repositories;

public class SubjectsTeachersRepository : Repository<SubjectTeacher>, ISubjectsTeachersRepository
{
    public SubjectsTeachersRepository(AppDbContext appDbContext) : base(appDbContext)
    {

    }
    public async Task DeleteTeacherSubjets(int teacherId)
    {
        await _dbContext.Set<SubjectTeacher>().Where(st => st.TeachersId == teacherId).ExecuteDeleteAsync();
    }
}
