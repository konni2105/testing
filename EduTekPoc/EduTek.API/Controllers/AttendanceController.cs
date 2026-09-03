using EduTek.Application.DTOs;
using EduTek.Application.Services;
using EduTek.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // GET: api/Attendance
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendanceRecords = await _attendanceService.GetAllAsync();

            var response = attendanceRecords.Select(a => new AttendanceDto
            {
                AttendanceId = a.AttendanceId,
                StudentId = a.StudentId,
                StudentName = $"{a.Student.FirstName} {a.Student.LastName}",
                SubjectId = a.SubjectId,
                SubjectName = a.Subject.SubjectName,
                AttendanceDate = a.AttendanceDate,
                IsPresent = a.IsPresent
            }).ToList();

            return Ok(response);
        }

        // GET: api/Attendance/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);

            if (attendance == null)
            {
                return NotFound(new
                {
                    message = "Attendance record not found."
                });
            }

            var response = new AttendanceDto
            {
                AttendanceId = attendance.AttendanceId,
                StudentId = attendance.StudentId,
                StudentName =
                    $"{attendance.Student.FirstName} {attendance.Student.LastName}",
                SubjectId = attendance.SubjectId,
                SubjectName = attendance.Subject.SubjectName,
                AttendanceDate = attendance.AttendanceDate,
                IsPresent = attendance.IsPresent
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAttendanceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacherAssigned =
                await _attendanceService.IsTeacherAssignedAsync(
                    dto.TeacherId,
                    dto.SubjectId,
                    dto.ClassId);

            if (!teacherAssigned)
            {
                return Forbid();
            }

            var studentInClass =
                await _attendanceService.IsStudentInClassAsync(
                    dto.StudentId,
                    dto.ClassId);

            if (!studentInClass)
            {
                return BadRequest(
                    "Student does not belong to the selected class.");
            }

            var attendance = new Attendance
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                AttendanceDate = dto.AttendanceDate,
                IsPresent = dto.IsPresent
            };

            var createdAttendance =
                await _attendanceService.AddAsync(attendance);

            return Ok(new
            {
                message = "Attendance created successfully.",
                attendanceId = createdAttendance.AttendanceId
            });
        }

        // PUT: api/Attendance/1
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateAttendanceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attendance = new Attendance
            {
                AttendanceDate = dto.AttendanceDate,
                IsPresent = dto.IsPresent
            };

            var updated =
                await _attendanceService.UpdateAsync(id, attendance);

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

        // DELETE: api/Attendance/1
        [Authorize(Roles = "Admin,Teacher")]
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
