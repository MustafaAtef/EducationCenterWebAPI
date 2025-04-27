using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using EducationCenter.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Secretary")]
public class ClassesController : ControllerBase
{
    private readonly IClassesService _classesService;
    private readonly IAttendanceService _attendanceService;
    public ClassesController(IClassesService classesService, IAttendanceService attendanceService)
    {
        _classesService = classesService;
        _attendanceService = attendanceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetAllClasses(int? weekOffset, int? gradeId)
    {
        var classes = await _classesService.GetAllClassesAsync(weekOffset ?? 0, gradeId);
        return Ok(classes);
    }

    [HttpGet("today")]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetTodayClasses()
    {
        var classes = await _classesService.GetTodayClassesAsync();
        return Ok(classes);
    }

    [HttpGet("{classId}")]
    public async Task<ActionResult<ClassAttendanceStatisticsDto>> GetClassAttendanceStatistics(int classId)
    {
        var classAttendanceStatistics = await _classesService.GetClassAttendanceStatisticsAsync(classId);
        return Ok(classAttendanceStatistics);
    }

    [HttpGet("today/attendance")]
    public async Task<ActionResult<IEnumerable<ClassStudent>>> GetTodayAttendanceAsync()
    {
        var classStudents = await _attendanceService.GetTodayAttendanceAsync();
        return Ok(classStudents);
    }

    [HttpPost]
    public async Task<ActionResult> CreateClass(CreateClassDto createClassDto)
    {
        await _classesService.CreateClassAsync(createClassDto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateClass(int id, UpdateClassDto updateClassDto)
    {
        updateClassDto.Id = id;
        await _classesService.UpdateClassAsync(updateClassDto);
        return Ok();

    }
    [HttpGet("{classId}/attendance")]
    public async Task<ActionResult<IEnumerable<ClassStudent>>> GetAttendanceAsync(int classId)
    {
        var classStudents = await _attendanceService.GetAttendanceAsync(classId);
        return Ok(classStudents);
    }

    [HttpPost("{classId}/attendance")]
    public async Task<ActionResult> RegisterStudentAttendanceAsync(int classId, RegisterStudentAttendanceAsyncDto RegisterStudentAttendanceAsyncDto)
    {
        RegisterStudentAttendanceAsyncDto.ClassId = classId;
        await _attendanceService.RegisterStudentAttendanceAsync(RegisterStudentAttendanceAsyncDto);
        return Ok();
    }
}
