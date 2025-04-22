using System;
using EducationCenterAPI.Database.Entities;

namespace EducationCenterAPI.RepositoryContracts;

public interface IStudentSubjectsTeachersRepository : IRepository<StudentSubjectsTeachers>
{
    Task DeleteByStudentId(int studentId);
}
