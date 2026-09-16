using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService 
            
            _attendanceService;

        public AttendanceController(
            IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

      [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendanceRecords =
                await _attendanceService.GetAllAsync();

            return Ok(attendanceRecords);
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance =
                await _attendanceService.GetByIdAsync(id);

            if (attendance == null)
            {
                return NotFound(new
                {
                    message = "Attendance record not found."
                });
            }

            return Ok(attendance);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateAttendanceDto dto)
        {
            var created =
                await _attendanceService.AddAsync(dto);

            return Ok(created);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateAttendanceDto dto)
        {
            var updated =
                await _attendanceService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Attendance record not found."
                });
            }

            return Ok(new
            {
                message = "Attendance updated successfully."
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _attendanceService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Attendance record not found."
                });
            }

            return Ok(new
            {
                message = "Attendance deleted successfully."
            });
        }
    }
}