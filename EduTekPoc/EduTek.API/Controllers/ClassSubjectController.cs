using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSubjectController : ControllerBase
    {
        private readonly IClassSubjectService _service;

        public ClassSubjectController(IClassSubjectService service)
        {
            _service = service;
        }

        // GET: api/ClassSubject
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classSubjects = await _service.GetAllAsync();

            var response = classSubjects.Select(cs => new
            {
                cs.ClassId,
                ClassName = cs.Class.ClassName,
                cs.SubjectId,
                SubjectName = cs.Subject.SubjectName
            });

            return Ok(response);
        }

        // GET: api/ClassSubject/1/2
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{classId}/{subjectId}")]
        public async Task<IActionResult> Get(
            int classId,
            int subjectId)
        {
            var classSubject =
                await _service.GetAsync(classId, subjectId);

            if (classSubject == null)
            {
                return NotFound("Class-Subject assignment not found.");
            }

            return Ok(new
            {
                classSubject.ClassId,
                ClassName = classSubject.Class.ClassName,
                classSubject.SubjectId,
                SubjectName = classSubject.Subject.SubjectName
            });
        }

        // POST: api/ClassSubject
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateClassSubjectDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existing =
                await _service.GetAsync(dto.ClassId, dto.SubjectId);

            if (existing != null)
            {
                return Conflict("This subject is already assigned to this class.");
            }

            var classSubject = new ClassSubject
            {
                ClassId = dto.ClassId,
                SubjectId = dto.SubjectId
            };

            await _service.AddAsync(classSubject);

            return Ok("Subject assigned to class successfully.");
        }

        // DELETE: api/ClassSubject/1/2
        [Authorize(Roles = "Admin")]
        [HttpDelete("{classId}/{subjectId}")]
        public async Task<IActionResult> Delete(
            int classId,
            int subjectId)
        {
            var deleted =
                await _service.DeleteAsync(classId, subjectId);

            if (!deleted)
            {
                return NotFound("Class-Subject assignment not found.");
            }

            return Ok("Subject removed from class successfully.");
        }
    }
}