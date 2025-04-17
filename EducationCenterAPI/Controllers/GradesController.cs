using EducationCenterAPI.Dtos;
using EducationCenterAPI.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenterAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly IGradesService _gradesService;
    public GradesController(IGradesService gradesService)
    {
        _gradesService = gradesService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGrade(CreateGradeDto createGradeDto)
    {
        await _gradesService.CreateGradeAsync(createGradeDto);
        return StatusCode(201);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GradeDto>>> GetAllGrades()
    {
        var grades = await _gradesService.GetAllGradesAsync();
        return Ok(grades);
    }

}
