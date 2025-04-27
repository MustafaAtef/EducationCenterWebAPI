using System;
using EducationCenter.Core.Entities;

namespace EducationCenter.Core.RepositoryContracts;

public interface IStudentSubjectsTeachersRepository : IRepository<StudentSubjectsTeachers>
{
    Task DeleteByStudentId(int studentId);
}
