using EducationCenterAPI.Database;
using EducationCenterAPI.Dtos;
using EducationCenterAPI.Exceptions;
using EducationCenterAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace EducationCenterAPI.Services
{
    public class GradesService : IGradesService
    {
        private readonly AppDbContext _appDbContext;

        public GradesService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task CreateGradeAsync(CreateGradeDto createGradeDto)
        {
            var grade = await _appDbContext.Grades.SingleOrDefaultAsync(g => g.Name == createGradeDto.Name);
            if (grade is not null) throw new UniqueException("Grade already exists");
            _appDbContext.Grades.Add(new() { Name = createGradeDto.Name });
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<GradeDto>> GetAllGradesAsync()
        {
            return await _appDbContext.Grades.Select(g => new GradeDto
            {
                Id = g.Id,
                Name = g.Name
            }).ToListAsync();
        }
    }
}
