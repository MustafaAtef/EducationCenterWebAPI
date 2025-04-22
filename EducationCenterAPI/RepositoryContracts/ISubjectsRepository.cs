using EducationCenterAPI.Database.Entities;

namespace EducationCenterAPI.RepositoryContracts;

public interface ISubjectsRepository : IRepository<Subject>
{
    Task DeleteSubjectByName(string name);
}
