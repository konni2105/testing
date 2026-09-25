using EduTek.Web.Filters;
using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    [SessionAuthorize]
    public class DashboardController : Controller
    {
        private readonly IApiService _apiService;

        public DashboardController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {


            var token = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }


            var username =
                HttpContext.Session.GetString("Username") ?? string.Empty;

            var role =
                HttpContext.Session.GetString("Role") ?? string.Empty;

            var studentResponse =
                await _apiService.GetAsync("api/Student");

            var teacherResponse =
                await _apiService.GetAsync("api/Teacher");

            var subjectResponse =
                await _apiService.GetAsync("api/Subject");

            var classResponse =
                await _apiService.GetAsync("api/Class");

            var students =
                JsonSerializer.Deserialize<List<object>>(
                    studentResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            var teachers =
                JsonSerializer.Deserialize<List<object>>(
                    teacherResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            var subjects =
                JsonSerializer.Deserialize<List<object>>(
                    subjectResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            var classes =
                JsonSerializer.Deserialize<List<object>>(
                    classResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            int pendingRegistrationCount = 0;

            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                var pendingResponse =
                    await _apiService.GetAsync("api/Admin/pending-registrations");

                var pendingUsers =
                    JsonSerializer.Deserialize<List<object>>(
                        pendingResponse,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                pendingRegistrationCount = pendingUsers?.Count ?? 0;
            }
            var model = new DashboardViewModel
            {
                Username = username,
                Role = role,

                StudentCount = students?.Count ?? 0,
                TeacherCount = teachers?.Count ?? 0,
                SubjectCount = subjects?.Count ?? 0,
                ClassCount = classes?.Count ?? 0,

                PendingRegistrationCount = pendingRegistrationCount
            };

            return View(model);
        }
    }
}