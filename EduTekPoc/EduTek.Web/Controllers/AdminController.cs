using EduTek.Web.Filters;
using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    [SessionAuthorize("Admin")]
    public class AdminController : Controller
    {
        private readonly IApiService _apiService;

        public AdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> PendingRegistrations()
        {
            var response =
                await _apiService.GetAsync(
                    "api/Admin/pending-registrations");

            var pendingUsers =
                JsonSerializer.Deserialize<List<PendingUserViewModel>>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(pendingUsers);
        }


        [HttpPost]
        public async Task<IActionResult> Approve(
                 int userId,
                 bool isApproved,
                 string username,
                 string email,
                 string role)
                    {
            var dto = new
            {
                UserId = userId,
                IsApproved = isApproved
            };

            try
            {
                await _apiService.PostAsync(
                    "api/Admin/approve",
                    dto);

                if (isApproved && role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction(
                        nameof(CreateStudent),
                        new
                        {
                            userId,
                            username,
                            email
                        });
                }

                TempData["Success"] = isApproved
                    ? "Registration approved successfully."
                    : "Registration rejected successfully.";

                return RedirectToAction(nameof(PendingRegistrations));
            }
            catch (HttpRequestException ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(PendingRegistrations));
            }
        }

        [HttpGet]
        public IActionResult CreateStudent(int userId, string username, string email)
        {
            var model = new CreateStudentViewModel
            {
                Email = email
            };

            ViewBag.UserId = userId;
            ViewBag.Username = username;

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateStudent(
    int userId,
    CreateStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = userId;
                return View(model);
            }

            try
            {
                await _apiService.PostAsync(
                    "api/Student",
                    model);

                TempData["Success"] =
                    "Student profile created successfully.";

                return RedirectToAction(nameof(PendingRegistrations));
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.UserId = userId;

                return View(model);
            }
        }
    }
}