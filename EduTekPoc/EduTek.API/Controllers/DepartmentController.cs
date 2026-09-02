using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/Department
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();

            var departmentDtos = departments.Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                Description = d.Description
            }).ToList();

            return Ok(departmentDtos);
        }

        // GET: api/Department/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            var departmentDto = new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Description = department.Description
            };

            return Ok(departmentDto);
        }

        // POST: api/Department
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = new Department
            {
                DepartmentName = dto.DepartmentName,
                Description = dto.Description
            };

            var createdDepartment = await _departmentService.AddAsync(department);

            var departmentDto = new DepartmentDto
            {
                DepartmentId = createdDepartment.DepartmentId,
                DepartmentName = createdDepartment.DepartmentName,
                Description = createdDepartment.Description
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdDepartment.DepartmentId },
                departmentDto);
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

            var department = new Department
            {
                DepartmentName = dto.DepartmentName,
                Description = dto.Description
            };

            var updated = await _departmentService.UpdateAsync(id, department);

            if (!updated)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            return Ok("Department updated successfully.");
        }

        // DELETE: api/Department/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _departmentService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound($"Department with ID {id} not found.");
            }

            return Ok("Department deleted successfully.");
        }
    }
}