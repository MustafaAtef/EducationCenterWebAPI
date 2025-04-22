using System;
using EducationCenterAPI.Database.Entities;

namespace EducationCenterAPI.RepositoryContracts;

public interface ISubjectsTeachersRepository : IRepository<SubjectTeacher>
{
    Task DeleteTeacherSubjets(int teacherId);

}
