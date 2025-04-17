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
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsService _studentsService;
        public StudentsController(IStudentsService studentService)
        {
            _studentsService = studentService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudent(CreateStudentDto createStudentDto)
        {
            await _studentsService.CreateStudentAsync(createStudentDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStudent(int id, UpdateStudentDto updateStudentDto)
        {
            updateStudentDto.Id = id;
            await _studentsService.UpdateStudentAsync(updateStudentDto);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<StudentDto>>> GetAllStudents(int page, int pageSize, string? searchItem, string? sortBy, string? sortOrder, int? grade)
        {
            return await _studentsService.GetAllStudentsAsync(page, pageSize, searchItem, sortBy, sortOrder, grade);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
            return await _studentsService.GetStudentByIdAsync(id);
        }

    }
}
