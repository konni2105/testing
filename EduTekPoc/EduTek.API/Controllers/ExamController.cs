using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        // GET: api/Exam
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _examService.GetAllAsync();

            return Ok(exams);
        }

        // GET: api/Exam/1
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exam = await _examService.GetByIdAsync(id);

            if (exam == null)
            {
                return NotFound(new
                {
                    message = "Exam not found."
                });
            }

            return Ok(exam);
        }

        // POST: api/Exam
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateExamDto dto)
        {
            var createdExam = await _examService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdExam.ExamId },
                createdExam);
        }

        // PUT: api/Exam/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateExamDto dto)
        {
            var updated =
                await _examService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Exam not found."
                });
            }

            return Ok(new
            {
                message = "Exam updated successfully."
            });
        }

        // DELETE: api/Exam/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _examService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Exam not found."
                });
            }

            return Ok(new
            {
                message = "Exam deleted successfully."
            });
        }
    }
}