using EduTek.Application.DTOs;
using EduTek.Application.Services;
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
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var marks = await _markService.GetAllAsync();

            return Ok(marks);
        }

        // GET: api/Mark/1
        [Authorize(Roles = "Admin,Teacher,Student")]
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

            return Ok(mark);
        }

        // POST: api/Mark
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateMarkDto dto)
        {
            var created =
                await _markService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.MarkId },
                created);
        }

        // PUT: api/Mark/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateMarkDto dto)
        {
            var updated =
                await _markService.UpdateAsync(id, dto);

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
            var deleted =
                await _markService.DeleteAsync(id);

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