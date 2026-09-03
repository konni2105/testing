using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/Feedback
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await _feedbackService.GetAllAsync();

            var response = feedbacks.Select(f => new FeedbackDto
            {
                FeedbackId = f.FeedbackId,
                TeacherId = f.TeacherId,
                TeacherName = $"{f.Teacher.FirstName} {f.Teacher.LastName}",
                StudentId = f.StudentId,
                StudentName = $"{f.Student.FirstName} {f.Student.LastName}",
                Comments = f.Comments,
                FeedbackDate = f.FeedbackDate
            }).ToList();

            return Ok(response);
        }

        // GET: api/Feedback/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);

            if (feedback == null)
            {
                return NotFound(new
                {
                    message = "Feedback not found."
                });
            }

            var response = new FeedbackDto
            {
                FeedbackId = feedback.FeedbackId,
                TeacherId = feedback.TeacherId,
                TeacherName =
                    $"{feedback.Teacher.FirstName} {feedback.Teacher.LastName}",
                StudentId = feedback.StudentId,
                StudentName =
                    $"{feedback.Student.FirstName} {feedback.Student.LastName}",
                Comments = feedback.Comments,
                FeedbackDate = feedback.FeedbackDate
            };

            return Ok(response);
        }

        // POST: api/Feedback
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateFeedbackDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = new Feedback
            {
                TeacherId = dto.TeacherId,
                StudentId = dto.StudentId,
                Comments = dto.Comments,
                FeedbackDate = dto.FeedbackDate
            };

            var created =
                await _feedbackService.AddAsync(feedback);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.FeedbackId },
                new
                {
                    message = "Feedback created successfully.",
                    feedbackId = created.FeedbackId
                });
        }

        // PUT: api/Feedback/1
        [Authorize(Roles = "Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateFeedbackDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = new Feedback
            {
                Comments = dto.Comments,
                FeedbackDate = dto.FeedbackDate
            };

            var updated =
                await _feedbackService.UpdateAsync(id, feedback);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Feedback not found."
                });
            }

            return Ok(new
            {
                message = "Feedback updated successfully."
            });
        }

        // DELETE: api/Feedback/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _feedbackService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Feedback not found."
                });
            }

            return Ok(new
            {
                message = "Feedback deleted successfully."
            });
        }
    }
}