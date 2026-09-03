using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly IMarkService _markService;

        public MarkController(IMarkService markService)
        {
            _markService = markService;
        }

        // GET: api/Mark
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var marks = await _markService.GetAllAsync();

            var response = marks.Select(m => new MarkDto
            {
                MarkId = m.MarkId,
                ExamId = m.ExamId,
                ExamName = m.Exam.ExamName,
                StudentId = m.StudentId,
                StudentName =
                    $"{m.Student.FirstName} {m.Student.LastName}",
                Score = m.Score
            }).ToList();

            return Ok(response);
        }

        // GET: api/Mark/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mark = await _markService.GetByIdAsync(id);

            if (mark == null)
            {
                return NotFound(new
                {
                    message = "Mark not found."
                });
            }

            var response = new MarkDto
            {
                MarkId = mark.MarkId,
                ExamId = mark.ExamId,
                ExamName = mark.Exam.ExamName,
                StudentId = mark.StudentId,
                StudentName =
                    $"{mark.Student.FirstName} {mark.Student.LastName}",
                Score = mark.Score
            };

            return Ok(response);
        }

        // POST: api/Mark
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMarkDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = await _markService.ExistsAsync(
                dto.ExamId,
                dto.StudentId);

            if (exists)
            {
                return Conflict(
                    "Marks already exist for this student and exam.");
            }

            var mark = new Mark
            {
                ExamId = dto.ExamId,
                StudentId = dto.StudentId,
                Score = dto.Score
            };

            var created = await _markService.AddAsync(mark);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.MarkId },
                new
                {
                    message = "Mark created successfully.",
                    markId = created.MarkId
                });
        }

        // PUT: api/Mark/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateMarkDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mark = new Mark
            {
                Score = dto.Score
            };

            var updated = await _markService.UpdateAsync(id, mark);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Mark not found."
                });
            }

            return Ok(new
            {
                message = "Mark updated successfully."
            });
        }

        // DELETE: api/Mark/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _markService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Mark not found."
                });
            }

            return Ok(new
            {
                message = "Mark deleted successfully."
            });
        }
    }
}