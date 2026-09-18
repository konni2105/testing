using Microsoft.AspNetCore.Mvc;

namespace EduTek.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");

            ViewBag.Username = username;
            ViewBag.Role = role;

            return View();
        }
    }
}