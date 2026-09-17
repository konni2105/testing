using EduTek.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EduTek.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        //  /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("EduTekAPI");

            var json = JsonSerializer.Serialize(model);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "api/Auth/login",
                content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid username or password.";
                return View(model);
            }

            var responseContent =
                await response.Content.ReadAsStringAsync();

            var authResponse = JsonSerializer.Deserialize<AuthResponseModel>(
                     responseContent,
                     new JsonSerializerOptions
                     {
                         PropertyNameCaseInsensitive = true
                     });

            if (authResponse == null)
            {
                ViewBag.Error = "Invalid response received from API.";
                return View(model);
            }

            HttpContext.Session.SetString(
                "AccessToken",
                authResponse.Token);

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
    }
}