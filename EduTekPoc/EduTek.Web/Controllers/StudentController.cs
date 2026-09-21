using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly IApiService _apiService;

        public StudentController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("api/Student");

            var students = JsonSerializer.Deserialize<List<StudentViewModel>>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(students);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _apiService.PostAsync("api/Student", model);

            TempData["Success"] = "Student created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _apiService.GetAsync($"api/Student/{id}");

            var student = JsonSerializer.Deserialize<StudentViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetAsync($"api/Student/{id}");

            var student = JsonSerializer.Deserialize<EditStudentViewModel>(
                response,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditStudentViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

          await _apiService.PutAsync($"api/Student/{model.StudentId}",model);

            
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync($"api/Student/{id}");

            return RedirectToAction(nameof(Index));
        }

    }
}