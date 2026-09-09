using EduTek.Application.DTOs;
using EduTek.Application.Services;
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

            return Ok(feedbacks);
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

            return Ok(feedback);
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

            var created =
                await _feedbackService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.FeedbackId },
                created);
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

            var updated =
                await _feedbackService.UpdateAsync(id, dto);

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