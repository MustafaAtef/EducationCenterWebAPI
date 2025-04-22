using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.RepositoryContracts;

namespace EducationCenterAPI.Repositories;

public class TeacherSalariesRepository : Repository<TeacherSalary>, ITeacherSalariesRepository
{
    public TeacherSalariesRepository(AppDbContext appDbContext) : base(appDbContext)
    {

    }

}
