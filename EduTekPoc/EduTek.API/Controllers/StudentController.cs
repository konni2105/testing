using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        // GET: api/Student
        [Authorize(Roles = "Admin,Teacher, Student")]
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _service.GetAllAsync();

            return Ok(students);
        }

        // GET: api/Student/5
        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _service.GetByIdAsync(id);

            if (student == null)
                return NotFound(new
                {
                    message = "Student not found."
                });

            return Ok(student);
        }

        // POST: api/Student
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateStudent(
            CreateStudentDto dto)
        {
            var createdStudent = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetStudent),
                new { id = createdStudent.StudentId },
                createdStudent);
        }

        // PUT: api/Student/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(
            int id,
            UpdateStudentDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
                return NotFound(new
                {
                    message = "Student not found."
                });

            return Ok(new
            {
                message = "Student updated successfully."
            });
        }

        // DELETE: api/Student/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound(new
                {
                    message = "Student not found."
                });

            return NoContent();
        }
    }
}