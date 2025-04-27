using EducationCenter.Application.Dtos;

namespace EducationCenter.Application.ServiceContracts;

public interface IGradesService
{
    Task CreateGradeAsync(CreateGradeDto createGradeDto);
    Task<IEnumerable<GradeDto>> GetAllGradesAsync();
}
