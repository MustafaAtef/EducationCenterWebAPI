using EducationCenterAPI.Dtos;
using EducationCenterAPI.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
}
