using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IApiService _apiService;

        public FeedbackController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Feedback
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/Feedback");

            var feedbacks = JsonSerializer.Deserialize<List<FeedbackViewModel>>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(feedbacks);
        }

        // GET: /Feedback/Details/1
        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync($"api/Feedback/{id}");

            var feedback = JsonSerializer.Deserialize<FeedbackViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: /Feedback/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Feedback/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateFeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync(
                "api/Feedback",
                model);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Feedback/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetAsync($"api/Feedback/{id}");

            var feedback = JsonSerializer.Deserialize<FeedbackViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (feedback == null)
            {
                return NotFound();
            }

            var model = new EditFeedbackViewModel
            {
                FeedbackId = feedback.FeedbackId,
                Comments = feedback.Comments,
                FeedbackDate = feedback.FeedbackDate
            };

            return View(model);
        }

        // POST: /Feedback/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(EditFeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PutAsync(
                $"api/Feedback/{model.FeedbackId}",
                model);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Feedback/Delete/1
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync(
                $"api/Feedback/{id}");

            return RedirectToAction(nameof(Index));
        }
    }
}