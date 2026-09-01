using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Authorize]
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
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _service.GetAllAsync();

            return Ok(students);
        }

        // GET: api/Student/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _service.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // POST: api/Student
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentDto dto)
        {
            var createdStudent = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetStudent),
                new { id = createdStudent.StudentId },
                createdStudent);
        }

        // PUT: api/Student/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(
            int id,
            UpdateStudentDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Student updated successfully");
        }

        // DELETE: api/Student/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("error")] //GET /api/Student/error
        public IActionResult TestError()
        {
            throw new Exception("Test exception");
        }


        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Welcome Admin! You have access to this endpoint.");
        }
    }
}