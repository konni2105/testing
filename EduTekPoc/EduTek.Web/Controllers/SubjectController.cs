using EduTek.Web.Filters;
using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    [SessionAuthorize]
    public class SubjectController : Controller
    {
        private readonly IApiService _apiService;

        public SubjectController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/Subject");

            var subjects = JsonSerializer.Deserialize<List<SubjectViewModel>>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(subjects);
        }

        // DETAILS - GET
        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync(
                $"api/Subject/{id}");

            var subject = JsonSerializer.Deserialize<SubjectViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // CREATE - GET
        [HttpGet]
        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(
            CreateSubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync(
                "api/Subject",
                model);

            return RedirectToAction(nameof(Index));
        }

        // EDIT - GET
        [HttpGet]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetAsync(
                $"api/Subject/{id}");

            var subject = JsonSerializer.Deserialize<EditSubjectViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // EDIT - POST
        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Edit(
            EditSubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PutAsync(
                $"api/Subject/{model.SubjectId}",
                model);

            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync(
                $"api/Subject/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}