using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonInfo.Models;
using System;
using System.Data;
using System.Security.Claims;

namespace PersonInfo.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.UserInfoAccounts.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel registrationViewModel)
        {
            if (ModelState.IsValid)
            {
                UserInfoAccount account = new UserInfoAccount();
                account.Name = registrationViewModel.Name;
                account.Email = registrationViewModel.Email;
                account.UserName = registrationViewModel.UserName;
                account.Password = registrationViewModel.Password;

                try
                {
                    _context.UserInfoAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear(); // this is clear the fields data.

                    ViewBag.Message = $"{account.Name} registered Successfully, Please login and check!!!";
                    return View();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, "this email already exits.");
                    throw;
                }
            }
            return View(registrationViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserInfoAccounts.Where(x => (x.UserName == loginViewModel.UserNameOrEmail || x.Email == loginViewModel.UserNameOrEmail) && x.Password == loginViewModel.Password).FirstOrDefault();
                //var user = _context.UserInfoAccounts.Where(x =>  x.Email == loginViewModel.UserNameOrEmail && x.Password == loginViewModel.Password).FirstOrDefault();
                if (user != null)
                {

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.UserName),
                        new Claim(ClaimTypes.Role, "user"),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("LandingPage");

                   // return RedirectToAction("LandingPage", "Account", new { username = loginViewModel.UserNameOrEmail });
                }
                else
                {
                    ModelState.AddModelError("", "UserName/Email or password is not correct.");
                }
            }
            return View();
        }

        //[Authorize]    //this is for once user success than landing to this page with cookies
        //public IActionResult SecurePage()
        //{
        //    ViewBag.Name = HttpContext.User.Identity.Name;
        //        return View();
        //}

        public IActionResult LandingPage()
        {
             ViewBag.Name = HttpContext.User.Identity.Name;
            //ViewBag.Name = username;
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

    }
}
