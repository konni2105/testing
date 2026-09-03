using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        // GET: api/Class
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classes = await _classService.GetAllAsync();

            var response = classes.Select(c => new ClassDto
            {
                ClassId = c.ClassId,
                ClassName = c.ClassName,
                Description = c.Description
            }).ToList();

            return Ok(response);
        }

        // GET: api/Class/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var classEntity = await _classService.GetByIdAsync(id);

            if (classEntity == null)
            {
                return NotFound(new
                {
                    message = "Class not found."
                });
            }

            var response = new ClassDto
            {
                ClassId = classEntity.ClassId,
                ClassName = classEntity.ClassName,
                Description = classEntity.Description
            };

            return Ok(response);
        }

        // POST: api/Class
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateClassDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var classEntity = new Class
            {
                ClassName = dto.ClassName,
                Description = dto.Description
            };

            var createdClass = await _classService.AddAsync(classEntity);

            var response = new ClassDto
            {
                ClassId = createdClass.ClassId,
                ClassName = createdClass.ClassName,
                Description = createdClass.Description
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdClass.ClassId },
                response);
        }

        // PUT: api/Class/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateClassDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var classEntity = new Class
            {
                ClassId = id,
                ClassName = dto.ClassName,
                Description = dto.Description
            };

            var updated = await _classService.UpdateAsync(id, classEntity);

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
            var deleted = await _classService.DeleteAsync(id);

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