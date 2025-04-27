using EducationCenter.Core;
using EducationCenter.Core.Exceptions;
using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using EducationCenter.Core.RepositoryContracts;

namespace EducationCenter.Application.Services;

public class GradesService : IGradesService
{
    private readonly IUnitOfWork _unitOfWork;
    public GradesService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateGradeAsync(CreateGradeDto createGradeDto)
    {
        var grade = await _unitOfWork.Grades.FindAsync(g => g.Name == createGradeDto.Name);
        if (grade is not null) throw new UniqueException("Grade already exists");
        _unitOfWork.Grades.Add(new() { Name = createGradeDto.Name });
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<GradeDto>> GetAllGradesAsync()
    {
        return (await _unitOfWork.Grades.GetAllAsync()).Select(g => new GradeDto
        {
            Id = g.Id,
            Name = g.Name
        });
    }
}
