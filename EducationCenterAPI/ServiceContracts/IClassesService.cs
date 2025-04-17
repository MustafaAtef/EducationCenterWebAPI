using System;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface IClassesService
{
    Task CreateClassAsync(CreateClassDto createClassDto);
    Task UpdateClassAsync(UpdateClassDto updateClassDto);
    Task<IEnumerable<ClassDto>> GetAllClasses(int weekOffset, int? GradeId);

}
