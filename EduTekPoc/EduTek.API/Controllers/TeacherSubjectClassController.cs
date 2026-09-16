using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherSubjectClassController : ControllerBase
    {
        private readonly ITeacherSubjectClassService _service;

        public TeacherSubjectClassController(
            ITeacherSubjectClassService service)
        {
            _service = service;
        }

        // GET: api/TeacherSubjectClass
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assignments = await _service.GetAllAsync();

            return Ok(assignments);
        }

        // GET: api/TeacherSubjectClass/1/1/1
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("{teacherId}/{subjectId}/{classId}")]
        public async Task<IActionResult> Get(
            int teacherId,
            int subjectId,
            int classId)
        {
            var assignment = await _service.GetAsync(
                teacherId,
                subjectId,
                classId);

            if (assignment == null)
            {
                return NotFound(
                    "Teacher-Subject-Class assignment not found.");
            }

            return Ok(assignment);
        }

        // POST: api/TeacherSubjectClass
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateTeacherSubjectClassDto dto)
        {

            await _service.AddAsync(dto);

            return Ok(
                "Teacher assigned to subject and class successfully.");
        }

        // DELETE: api/TeacherSubjectClass/1/1/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{teacherId}/{subjectId}/{classId}")]
        public async Task<IActionResult> Delete(
            int teacherId,
            int subjectId,
            int classId)
        {
            var deleted = await _service.DeleteAsync(
                teacherId,
                subjectId,
                classId);

            if (!deleted)
            {
                return NotFound(
                    "Teacher-Subject-Class assignment not found.");
            }

            return Ok(
                "Teacher assignment removed successfully.");
        }
    }
}