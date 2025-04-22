using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.RepositoryContracts;
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
