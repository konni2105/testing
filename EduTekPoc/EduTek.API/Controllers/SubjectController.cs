using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(
            ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: api/Subject
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjects =
                await _subjectService.GetAllAsync();

            return Ok(subjects);
        }

        // GET: api/Subject/5
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subject =
                await _subjectService.GetByIdAsync(id);

            if (subject == null)
            {
                return NotFound(new
                {
                    message = "Subject not found."
                });
            }

            return Ok(subject);
        }

        // POST: api/Subject
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateSubjectDto dto)
        {
            var createdSubject =
                await _subjectService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdSubject.SubjectId },
                createdSubject);
        }

        // PUT: api/Subject/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateSubjectDto dto)
        {
            var updated =
                await _subjectService.UpdateAsync(id, dto);

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
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _subjectService.DeleteAsync(id);

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