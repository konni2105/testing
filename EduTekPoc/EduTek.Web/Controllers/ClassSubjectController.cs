using EduTek.Web.Filters;
using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    [SessionAuthorize]
    public class ClassSubjectController : Controller
    {
        private readonly IApiService _apiService;

        public ClassSubjectController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /ClassSubject
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/ClassSubject");

            var classSubjects =
                JsonSerializer.Deserialize<List<ClassSubjectViewModel>>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(classSubjects);
        }

        // GET: /ClassSubject/Create
        [HttpGet]
        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /ClassSubject/Create
        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(
            CreateClassSubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _apiService.PostAsync(
                    "api/ClassSubject",
                    model);

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // POST: /ClassSubject/Delete
        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(
            int classId,
            int subjectId)
        {
            var endpoint =
                $"api/ClassSubject/{classId}/{subjectId}";

            var response =
                await _apiService.DeleteAsync(endpoint);

            if (response.StartsWith("2"))
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = response;

            return RedirectToAction(nameof(Index));
        }
    }
}