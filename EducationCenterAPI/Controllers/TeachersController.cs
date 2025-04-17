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
    public class TeachersController : ControllerBase
    {
        private readonly ITeachersService _teachersService;
        public TeachersController(ITeachersService teacherService)
        {
            _teachersService = teacherService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTeacher(CreateTeacherDto createTeacherDto)
        {
            await _teachersService.CreateTeacherAsync(createTeacherDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeacher(int id, UpdateTeacherDto updateTeacherDto)
        {
            updateTeacherDto.Id = id;
            await _teachersService.UpdateTeacherAsync(updateTeacherDto);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<TeacherDto>>> GetTeachers(int page = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, string? sortOrder = null, string? subject = null)
        {
            return await _teachersService.GetTeachersAsync(page, pageSize, searchTerm, sortBy, sortOrder, subject);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDto>> GetTeacherById(int id)
        {
            return await _teachersService.GetTeacherByIdAsync(id);
        }
    }

}
