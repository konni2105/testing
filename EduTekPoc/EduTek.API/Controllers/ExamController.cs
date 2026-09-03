using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
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
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _examService.GetAllAsync();

            var response = exams.Select(e => new ExamDto
            {
                ExamId = e.ExamId,
                ExamName = e.ExamName,
                SubjectId = e.SubjectId,
                SubjectName = e.Subject.SubjectName,
                ClassId = e.ClassId,
                ClassName = e.Class.ClassName,
                ExamDate = e.ExamDate
            }).ToList();

            return Ok(response);
        }

        // GET: api/Exam/1
        [Authorize(Roles = "Admin,Teacher")]
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

            var response = new ExamDto
            {
                ExamId = exam.ExamId,
                ExamName = exam.ExamName,
                SubjectId = exam.SubjectId,
                SubjectName = exam.Subject.SubjectName,
                ClassId = exam.ClassId,
                ClassName = exam.Class.ClassName,
                ExamDate = exam.ExamDate
            };

            return Ok(response);
        }

        // POST: api/Exam
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateExamDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = new Exam
            {
                ExamName = dto.ExamName,
                SubjectId = dto.SubjectId,
                ClassId = dto.ClassId,
                ExamDate = dto.ExamDate
            };

            var createdExam = await _examService.AddAsync(exam);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdExam.ExamId },
                new
                {
                    message = "Exam created successfully.",
                    examId = createdExam.ExamId
                });
        }

        // PUT: api/Exam/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateExamDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = new Exam
            {
                ExamId = id,
                ExamName = dto.ExamName,
                SubjectId = dto.SubjectId,
                ClassId = dto.ClassId,
                ExamDate = dto.ExamDate
            };

            var updated = await _examService.UpdateAsync(id, exam);

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
            var deleted = await _examService.DeleteAsync(id);

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