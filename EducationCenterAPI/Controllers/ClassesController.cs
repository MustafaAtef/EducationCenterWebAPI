using EducationCenterAPI.Dtos;
using EducationCenterAPI.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesService _classesService;
        public ClassesController(IClassesService classesService)
        {
            _classesService = classesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetAllClasses(int? weekOffset, int? gradeId)
        {
            var classes = await _classesService.GetAllClasses(weekOffset ?? 0, gradeId);
            return Ok(classes);
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
    }
}
