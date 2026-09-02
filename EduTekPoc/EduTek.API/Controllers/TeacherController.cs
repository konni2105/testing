using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllAsync();

            var response = teachers.Select(t => new TeacherDto
            {
                TeacherId = t.TeacherId,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Subject = t.Subject
            });

            return Ok(response);
        }

        // GET: api/Teacher/5
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

            var response = new TeacherDto
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                Subject = teacher.Subject
            };

            return Ok(response);
        }

        // POST: api/Teacher
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeacherDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // DTO → Entity
            var teacher = new Teacher
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Subject = dto.Subject
            };

            var createdTeacher = await _teacherService.AddAsync(teacher);

            // Entity → Response DTO
            var response = new TeacherDto
            {
                TeacherId = createdTeacher.TeacherId,
                FirstName = createdTeacher.FirstName,
                LastName = createdTeacher.LastName,
                Email = createdTeacher.Email,
                PhoneNumber = createdTeacher.PhoneNumber,
                Subject = createdTeacher.Subject
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdTeacher.TeacherId },
                response);
        }

        // PUT: api/Teacher/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateTeacherDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // DTO → Entity
            var teacher = new Teacher
            {
                TeacherId = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Subject = dto.Subject
            };

            var updated = await _teacherService.UpdateAsync(id, teacher);

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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _teacherService.DeleteAsync(id);

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