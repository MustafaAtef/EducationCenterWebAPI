
using EducationCenter.Core.Entities;

namespace EducationCenter.Core.RepositoryContracts;

public interface ISubjectsRepository : IRepository<Subject>
{
    Task DeleteSubjectByName(string name);
}
