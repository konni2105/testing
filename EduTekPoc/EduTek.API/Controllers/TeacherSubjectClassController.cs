using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
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
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assignments = await _service.GetAllAsync();

            var response = assignments.Select(x => new
            {
                x.TeacherId,
                TeacherName = $"{x.Teacher.FirstName} {x.Teacher.LastName}",

                x.SubjectId,
                SubjectName = x.Subject.SubjectName,

                x.ClassId,
                ClassName = x.Class.ClassName
            });

            return Ok(response);
        }

        // GET: api/TeacherSubjectClass/1/1/1
        [Authorize(Roles = "Admin,Teacher")]
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

            return Ok(new
            {
                assignment.TeacherId,
                TeacherName =
                    $"{assignment.Teacher.FirstName} {assignment.Teacher.LastName}",

                assignment.SubjectId,
                SubjectName =
                    assignment.Subject.SubjectName,

                assignment.ClassId,
                ClassName =
                    assignment.Class.ClassName
            });
        }

        // POST: api/TeacherSubjectClass
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateTeacherSubjectClassDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing = await _service.GetAsync(
                dto.TeacherId,
                dto.SubjectId,
                dto.ClassId);

            if (existing != null)
            {
                return Conflict(
                    "This teacher is already assigned to this subject and class.");
            }

            var assignment = new TeacherSubjectClass
            {
                TeacherId = dto.TeacherId,
                SubjectId = dto.SubjectId,
                ClassId = dto.ClassId
            };

            await _service.AddAsync(assignment);

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