using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectsService _subjectsService;
    public SubjectsController(ISubjectsService subjectService)
    {
        _subjectsService = subjectService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetAllSubjects(int? gradeId)
    {
        var subjects = await _subjectsService.GetAllSubjectsAsync(gradeId);
        return Ok(subjects);
    }

    [HttpPost]
    public async Task<ActionResult> CreateSubject(CreateSubjectDto createSubjectDto)
    {
        await _subjectsService.CreateSubjectAsync(createSubjectDto);
        return StatusCode(201);
    }
    [HttpPut]
    public async Task<ActionResult> UpdateSubject(UpdateSubjectDto updateSubjectDto)
    {
        await _subjectsService.UpdateSubjectAsync(updateSubjectDto);
        return StatusCode(201);
    }
}
