using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(
            IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/Department
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments =
                await _departmentService.GetAllAsync();

            return Ok(departments);
        }

        // GET: api/Department/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department =
                await _departmentService.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(
                    $"Department with ID {id} not found.");
            }

            return Ok(department);
        }

        // POST: api/Department
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdDepartment =
                await _departmentService.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdDepartment.DepartmentId },
                createdDepartment);
        }

        // PUT: api/Department/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated =
                await _departmentService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(
                    $"Department with ID {id} not found.");
            }

            return Ok(
                "Department updated successfully.");
        }

        // DELETE: api/Department/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _departmentService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(
                    $"Department with ID {id} not found.");
            }

            return Ok(
                "Department deleted successfully.");
        }
    }
}