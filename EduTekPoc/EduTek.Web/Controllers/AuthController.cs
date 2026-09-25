using EduTek.Web.Models;
using EduTek.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiService _apiService;

        public AuthController(
            IHttpClientFactory httpClientFactory,
            IApiService apiService)
        {
            _httpClientFactory = httpClientFactory;
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AccessToken")))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("EduTekAPI");

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponseModel>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
            {
                ModelState.AddModelError(string.Empty, "Invalid response received from API.");
                return View(model);
            }

            HttpContext.Session.SetString("AccessToken", authResponse.Token);
            HttpContext.Session.SetString("RefreshToken", authResponse.RefreshToken);
            HttpContext.Session.SetString("Username", authResponse.Username);
            HttpContext.Session.SetString("Role", authResponse.Role);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("EduTekAPI");
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Auth/register", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = await response.Content.ReadAsStringAsync();
                return View(model);
            }

            ViewBag.Success = "Registration successful. Please wait for Admin approval.";
            ModelState.Clear();
            return View();
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var response = await _apiService.RefreshTokenAsync(refreshToken);
            return Content(response, "application/json");
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (!string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    await _apiService.LogoutAsync(refreshToken);
                }
                catch (Exception)
                {
                    // Session is still cleared even if API revocation fails.
                }
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
