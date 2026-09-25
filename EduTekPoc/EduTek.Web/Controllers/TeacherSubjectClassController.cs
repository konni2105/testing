using EduTek.Web.Filters;
using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    [SessionAuthorize]
    public class TeacherSubjectClassController : Controller
    {
        private readonly IApiService _apiService;

        public TeacherSubjectClassController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /TeacherSubjectClass
        public async Task<IActionResult> Index()
        {
            var response =
                await _apiService.GetAsync("api/TeacherSubjectClass");

            var teacherSubjectClasses =
                JsonSerializer.Deserialize<List<TeacherSubjectClassViewModel>>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return View(teacherSubjectClasses);
        }

        // GET: /TeacherSubjectClass/Create
        [HttpGet]
        [SessionAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /TeacherSubjectClass/Create
        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Create(
            CreateTeacherSubjectClassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _apiService.PostAsync(
                    "api/TeacherSubjectClass",
                    model);

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = ex.Message;

                return View(model);
            }
        }

        // POST: /TeacherSubjectClass/Delete
        [HttpPost]
        [SessionAuthorize("Admin")]
        public async Task<IActionResult> Delete(
            int teacherId,
            int subjectId,
            int classId)
        {
            var endpoint =
                $"api/TeacherSubjectClass/{teacherId}/{subjectId}/{classId}";

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