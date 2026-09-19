using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    public class MarkController : Controller
    {
        private readonly IApiService _apiService;

        public MarkController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Mark
        public async Task<IActionResult> Index()
        {
            var response =
                await _apiService.GetAsync("api/Mark");

            var marks =
                JsonSerializer.Deserialize<List<MarkViewModel>>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(marks);
        }

        // GET: /Mark/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var response =
                await _apiService.GetAsync($"api/Mark/{id}");

            var mark =
                JsonSerializer.Deserialize<MarkViewModel>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: /Mark/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Mark/Create
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateMarkViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync(
                "api/Mark",
                model);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Mark/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response =
                await _apiService.GetAsync($"api/Mark/{id}");

            var mark =
                JsonSerializer.Deserialize<MarkViewModel>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (mark == null)
            {
                return NotFound();
            }

            var model = new EditMarkViewModel
            {
                MarkId = mark.MarkId,
                Score = mark.Score
            };

            return View(model);
        }

        // POST: /Mark/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(
            EditMarkViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PutAsync(
                $"api/Mark/{model.MarkId}",
                model);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Mark/Delete/1
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync(
                $"api/Mark/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}