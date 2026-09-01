using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _subjectService.GetAllAsync();

            var response = subjects.Select(s => new SubjectDto
            {
                SubjectId = s.SubjectId,
                SubjectName = s.SubjectName,
                Description = s.Description
            });

            return Ok(response);
        }

        // GET: api/Subject/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);

            if (subject == null)
            {
                return NotFound(new
                {
                    message = "Subject not found."
                });
            }

            var response = new SubjectDto
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                Description = subject.Description
            };

            return Ok(response);
        }

        // POST: api/Subject
        [HttpPost]
        public async Task<IActionResult> Create(CreateSubjectDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // DTO → Entity
            var subject = new Subject
            {
                SubjectName = dto.SubjectName,
                Description = dto.Description
            };

            var createdSubject = await _subjectService.AddAsync(subject);

            // Entity → Response DTO
            var response = new SubjectDto
            {
                SubjectId = createdSubject.SubjectId,
                SubjectName = createdSubject.SubjectName,
                Description = createdSubject.Description
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdSubject.SubjectId },
                response);
        }

        // PUT: api/Subject/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateSubjectDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // DTO → Entity
            var subject = new Subject
            {
                SubjectId = id,
                SubjectName = dto.SubjectName,
                Description = dto.Description
            };

            var updated = await _subjectService.UpdateAsync(id, subject);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Subject not found."
                });
            }

            return Ok(new
            {
                message = "Subject updated successfully."
            });
        }

        // DELETE: api/Subject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _subjectService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Subject not found."
                });
            }

            return Ok(new
            {
                message = "Subject deleted successfully."
            });
        }
    }
}
