using EduTek.Web.Filters;
using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    [SessionAuthorize]
    public class TeacherController : Controller
    {
        private readonly IApiService _apiService;

        public TeacherController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/Teacher");

            var teachers = JsonSerializer.Deserialize<List<TeacherViewModel>>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(teachers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync($"api/Teacher/{id}");

            var teacher = JsonSerializer.Deserialize<TeacherViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpGet]
        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(CreateTeacherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync("api/Teacher", model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetAsync($"api/Teacher/{id}");

            var teacher = JsonSerializer.Deserialize<EditTeacherViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (teacher == null)
            {
                return NotFound();
            }

            teacher.TeacherId = id;

            return View(teacher);
        }

        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(EditTeacherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PutAsync(
                $"api/Teacher/{model.TeacherId}",
                model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"api/Teacher/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}