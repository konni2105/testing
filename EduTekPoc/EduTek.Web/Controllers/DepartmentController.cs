using EduTek.Web.Filters;
using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    [SessionAuthorize]
    public class DepartmentController : Controller
    {
        private readonly IApiService _apiService;

        public DepartmentController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/Department");

            var departments = JsonSerializer.Deserialize<List<DepartmentViewModel>>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(departments);
        }

        [HttpGet]
        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(CreateDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync(
                "api/Department",
                model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync(
                $"api/Department/{id}");

            var department = JsonSerializer.Deserialize<DepartmentViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpGet]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetAsync(
                $"api/Department/{id}");

            var department = JsonSerializer.Deserialize<EditDepartmentViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(EditDepartmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PutAsync(
                $"api/Department/{model.DepartmentId}",
                model);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync(
                $"api/Department/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}