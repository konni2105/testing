using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    public class ExamController : Controller
    {
        private readonly IApiService _apiService;

        public ExamController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Exam
        public async Task<IActionResult> Index()
        {
            var response =
                await _apiService.GetAsync("api/Exam");

            var exams =
                JsonSerializer.Deserialize<List<ExamViewModel>>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(exams);
        }

        // GET: /Exam/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var response =
                await _apiService.GetAsync($"api/Exam/{id}");

            var exam =
                JsonSerializer.Deserialize<ExamViewModel>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // GET: /Exam/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Exam/Create
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync(
                "api/Exam",
                model);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Exam/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response =
                await _apiService.GetAsync($"api/Exam/{id}");

            var exam =
                JsonSerializer.Deserialize<ExamViewModel>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (exam == null)
            {
                return NotFound();
            }

            var model = new EditExamViewModel
            {
                ExamId = exam.ExamId,
                ExamName = exam.ExamName,
                ExamDate = exam.ExamDate
            };

            return View(model);
        }

        // POST: /Exam/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(
            EditExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PutAsync(
                $"api/Exam/{model.ExamId}",
                model);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Exam/Delete/1
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync(
                $"api/Exam/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}