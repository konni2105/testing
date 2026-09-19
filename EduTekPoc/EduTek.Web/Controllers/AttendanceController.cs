using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IApiService _apiService;

        public AttendanceController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response =
                await _apiService.GetAsync("api/Attendance");

            var attendance =
                JsonSerializer.Deserialize<List<AttendanceViewModel>>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(attendance);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response =
                await _apiService.GetAsync($"api/Attendance/{id}");

            var attendance =
                JsonSerializer.Deserialize<AttendanceViewModel>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateAttendanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync(
                "api/Attendance",
                model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response =
                await _apiService.GetAsync($"api/Attendance/{id}");

            var attendance =
                JsonSerializer.Deserialize<EditAttendanceViewModel>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (attendance == null)
            {
                return NotFound();
            }

            attendance.AttendanceId = id;

            return View(attendance);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            EditAttendanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PutAsync(
                $"api/Attendance/{model.AttendanceId}",
                model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync(
                $"api/Attendance/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}