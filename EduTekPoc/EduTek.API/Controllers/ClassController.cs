using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(
            IClassService classService)
        {
            _classService = classService;
        }

        // GET: api/Class
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classes =
                await _classService.GetAllAsync();

            return Ok(classes);
        }

        // GET: api/Class/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var classEntity =
                await _classService.GetByIdAsync(id);

            if (classEntity == null)
            {
                return NotFound(new
                {
                    message = "Class not found."
                });
            }

            return Ok(classEntity);
        }

        // POST: api/Class
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateClassDto dto)
        {
            var createdClass =
                await _classService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdClass.ClassId },
                createdClass);
        }

        // PUT: api/Class/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateClassDto dto)
        {
            var updated =
                await _classService.UpdateAsync(
                    id,
                    dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Class not found."
                });
            }

            return Ok(new
            {
                message = "Class updated successfully."
            });
        }

        // DELETE: api/Class/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _classService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Class not found."
                });
            }

            return Ok(new
            {
                message = "Class deleted successfully."
            });
        }
    }
}