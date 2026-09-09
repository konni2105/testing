using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSubjectController : ControllerBase
    {
        private readonly IClassSubjectService _service;

        public ClassSubjectController(
            IClassSubjectService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classSubjects =
                await _service.GetAllAsync();

            return Ok(classSubjects);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{classId}/{subjectId}")]
        public async Task<IActionResult> Get(
            int classId,
            int subjectId)
        {
            var classSubject =
                await _service.GetAsync(
                    classId,
                    subjectId);

            if (classSubject == null)
            {
                return NotFound(new
                {
                    message =
                        "Class-Subject assignment not found."
                });
            }

            return Ok(classSubject);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateClassSubjectDto dto)
        {
            var created =
                await _service.AddAsync(dto);

            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{classId}/{subjectId}")]
        public async Task<IActionResult> Delete(
            int classId,
            int subjectId)
        {
            var deleted =
                await _service.DeleteAsync(
                    classId,
                    subjectId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        "Class-Subject assignment not found."
                });
            }

            return Ok(new
            {
                message =
                    "Subject removed from class successfully."
            });
        }
    }
}