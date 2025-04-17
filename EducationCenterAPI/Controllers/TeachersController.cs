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

        [HttpPost("{teacherId}/salaries")]
        public async Task<ActionResult> PayTeacherSalary(int teacherId, PayTeacherSalaryDto payTeacherSalaryDto)
        {
            payTeacherSalaryDto.TeacherId = teacherId;
            await _teachersService.PayTeacherSalaryAsync(payTeacherSalaryDto);
            return Ok();
        }

        [HttpGet("{teacherId}/salaries")]
        public async Task<ActionResult<PagedList<TeacherSalaryDto>>> GetTeacherSalaries(int teacherId, int page = 1, int pageSize = 10)
        {
            return await _teachersService.GetTeacherSalariesAsync(teacherId, page, pageSize);
        }

        [HttpPut("{teacherId}/salaries/{id}")]
        public async Task<ActionResult> UpdateTeacherSalary(int teacherId, int id, UpdateTeacherSalaryDto updateTeacherSalaryDto)
        {
            updateTeacherSalaryDto.TeacherId = teacherId;
            updateTeacherSalaryDto.Id = id;
            await _teachersService.UpdateTeacherSalaryAsync(updateTeacherSalaryDto);
            return Ok();
        }

        [HttpGet("salaries")]
        public async Task<ActionResult<PagedList<TeacherSalaryDto>>> GetTeachersSalaries(int page = 1, int pageSize = 10, string? sortBy = null, string? sortOrder = null, string? fromDate = null, string? toDate = null)
        {
            return await _teachersService.GetTeachersSalariesAsync(page, pageSize, sortBy, sortOrder, fromDate, toDate);
        }
    }

}
