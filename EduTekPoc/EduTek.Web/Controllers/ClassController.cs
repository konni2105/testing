using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    public class ClassController : Controller
    {
        private readonly IApiService _apiService;

        public ClassController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Class
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/Class");

            var classes = JsonSerializer.Deserialize<List<ClassViewModel>>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(classes);
        }

        // GET: /Class/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync($"api/Class/{id}");

            var classItem = JsonSerializer.Deserialize<ClassViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (classItem == null)
            {
                return NotFound();
            }

            return View(classItem);
        }

        // GET: /Class/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Class/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync("api/Class", model);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Class/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetAsync($"api/Class/{id}");

            var classItem = JsonSerializer.Deserialize<EditClassViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (classItem == null)
            {
                return NotFound();
            }

            return View(classItem);
        }

        // POST: /Class/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(EditClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PutAsync(
                $"api/Class/{model.ClassId}",
                model);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Class/Delete/1
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"api/Class/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}