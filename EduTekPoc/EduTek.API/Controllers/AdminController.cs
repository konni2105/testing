using EduTek.Application.DTOs;
using EduTek.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTek.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("pending-registrations")]
        public async Task<IActionResult> GetPendingRegistrations()
        {
            var users =
                await _adminService.GetPendingRegistrationsAsync();

            return Ok(users);
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveRegistration(
            ApprovalDto dto)
        {
            var result =
                await _adminService.ApproveRegistrationAsync(dto);

            if (!result)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            return Ok(new
            {
                message = dto.IsApproved
                    ? "Registration approved successfully."
                    : "Registration rejected successfully."
            });
        }

        [HttpPost("bulk-approve")]
        public async Task<IActionResult> BulkApproveRegistrations(
                [FromBody] BulkApprovalDto dto)
        {
            var result = await _adminService.BulkApproveRegistrationsAsync(dto);

            return Ok(new
            {
                message = dto.IsApproved
                    ? "Registrations approved successfully."
                    : "Registrations rejected successfully."
            });
        }
    }
}