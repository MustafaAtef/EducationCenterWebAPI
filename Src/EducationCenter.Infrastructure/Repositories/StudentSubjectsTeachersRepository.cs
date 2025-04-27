using EducationCenter.Core.Entities;
using EducationCenter.Core.RepositoryContracts;
using EducationCenter.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Repositories;

public class StudentSubjectsTeachersRepository : Repository<StudentSubjectsTeachers>, IStudentSubjectsTeachersRepository
{
    public StudentSubjectsTeachersRepository(AppDbContext context) : base(context)
    {
    }

    public async Task DeleteByStudentId(int studentId)
    {
        await _dbContext.Set<StudentSubjectsTeachers>()
            .Where(s => s.StudentId == studentId)
            .ExecuteDeleteAsync();
    }

}
