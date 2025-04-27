using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Secretary")]
public class SubjectsWithTeachersController : ControllerBase
{
    private readonly ISubjectsService _subjectsService;
    public SubjectsWithTeachersController(ISubjectsService subjectsService)
    {
        _subjectsService = subjectsService;
    }

    [HttpGet("grade/{id}")]
    public async Task<ActionResult<IEnumerable<SubjectWithTeacherDto>>> GetSubjectsWithTeachersByGradeId(int id)
    {
        var subjects = await _subjectsService.GetSubjectsWithTeachersByGradeIdAsync(id);
        return Ok(subjects);
    }
}
