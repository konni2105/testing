using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // GET: api/Teacher
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllAsync();

            return Ok(teachers);
        }

        // GET: api/Teacher/5
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);

            if (teacher == null)
            {
                return NotFound(new
                {
                    message = "Teacher not found."
                });
            }

            return Ok(teacher);
        }

        // POST: api/Teacher
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateTeacherDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTeacher =
                await _teacherService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdTeacher.TeacherId },
                createdTeacher);
        }

        // PUT: api/Teacher/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateTeacherDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated =
                await _teacherService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Teacher not found."
                });
            }

            return Ok(new
            {
                message = "Teacher updated successfully."
            });
        }

        // DELETE: api/Teacher/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _teacherService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Teacher not found."
                });
            }

            return Ok(new
            {
                message = "Teacher deleted successfully."
            });
        }
    }
}