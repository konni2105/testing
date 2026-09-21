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


        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client =
                _httpClientFactory.CreateClient("EduTekAPI");

            var json =
                JsonSerializer.Serialize(model);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "api/Auth/login",
                content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error =
                    "Invalid username or password.";

                return View(model);
            }

            var responseContent =
                await response.Content.ReadAsStringAsync();

            var authResponse =
                JsonSerializer.Deserialize<AuthResponseModel>(
                    responseContent,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (authResponse == null)
            {
                ViewBag.Error =
                    "Invalid response received from API.";

                return View(model);
            }

            // Store authentication information in Session
            HttpContext.Session.SetString(
                "AccessToken",
                authResponse.Token);

            HttpContext.Session.SetString(
                "RefreshToken",
                authResponse.RefreshToken);

            HttpContext.Session.SetString(
                "Username",
                authResponse.Username);

            HttpContext.Session.SetString(
                "Role",
                authResponse.Role);

            ViewBag.Username = authResponse.Username;
            ViewBag.Role = authResponse.Role;
            ViewBag.Expiration = authResponse.Expiration;

            return View(model);
        }


        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client =
                _httpClientFactory.CreateClient("EduTekAPI");

            var json =
                JsonSerializer.Serialize(model);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "api/Auth/register",
                content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error =
                    await response.Content.ReadAsStringAsync();

                return View(model);
            }

            ViewBag.Success =
                "Registration successful. Please wait for Admin approval.";

            ModelState.Clear();

            return View();
        }


        // POST: /Auth/RefreshToken
        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken =
                HttpContext.Session.GetString("RefreshToken");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var response =
                await _apiService.RefreshTokenAsync(
                    refreshToken);

            return Content(
                response,
                "application/json");
        }


        // GET: /Auth/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Login",
                "Auth");
        }
    }
}