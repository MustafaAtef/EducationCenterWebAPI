using EducationCenterAPI.Dtos;
using EducationCenterAPI.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
}
