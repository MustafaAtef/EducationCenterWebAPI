using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Secretary")]
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

    [HttpPost("{studentId}/fees")]
    public async Task<ActionResult> PayStudentFees(int studentId, PayStudentFeesDto payStudentFeesDto)
    {
        payStudentFeesDto.StudentId = studentId;
        await _studentsService.PayStudentFeesAsync(payStudentFeesDto);
        return Ok();
    }

    [HttpGet("fees")]
    public async Task<ActionResult<PagedList<StudentFeeDto>>> GetStudentsFees(int page, int pageSize, string? sortBy, string? sortOrder, string? fromDate, string? toDate)
    {
        return await _studentsService.GetStudentsFeesAsync(page, pageSize, sortBy, sortOrder, fromDate, toDate);
    }

    [HttpGet("{studentId}/fees")]
    public async Task<ActionResult<PagedList<StudentFeeDto>>> GetStudentFees(int studentId, int page, int pageSize)
    {
        return await _studentsService.GetStudentFeesAsync(studentId, page, pageSize);
    }

    [HttpPut("{studentId}/fees/{expenseId}")]
    public async Task<ActionResult> UpdateStudentFees(int studentId, int expenseId, UpdateStudentFeesDto updateStudentFeesDto)
    {
        updateStudentFeesDto.StudentId = studentId;
        updateStudentFeesDto.Id = expenseId;
        await _studentsService.UpdateStudentFeesAsync(updateStudentFeesDto);
        return Ok();
    }
}
