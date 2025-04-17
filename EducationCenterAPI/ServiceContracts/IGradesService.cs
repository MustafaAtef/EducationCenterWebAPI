using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts;

public interface IGradesService
{
    Task CreateGradeAsync(CreateGradeDto createGradeDto);
    Task<IEnumerable<GradeDto>> GetAllGradesAsync();
}
