using System;
using EducationCenter.Core.Entities;

namespace EducationCenter.Core.RepositoryContracts;

public interface ISubjectsTeachersRepository : IRepository<SubjectTeacher>
{
    Task DeleteTeacherSubjets(int teacherId);

}
