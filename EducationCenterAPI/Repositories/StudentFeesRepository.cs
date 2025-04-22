using System;
using EducationCenterAPI.Database;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.RepositoryContracts;

namespace EducationCenterAPI.Repositories;

public class StudentFeesRepository : Repository<StudentFee>, IStudentFeesRepository
{
    public StudentFeesRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
}
