using EducationCenterAPI.Dtos;
using EducationCenterAPI.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet("class/{classId}")]
        public async Task<ActionResult<IEnumerable<ClassStudentDto>>> GetAttendance(int classId)
        {
            var classStudents = await _attendanceService.GetAttendance(classId);
            return Ok(classStudents);
        }
    }
}
