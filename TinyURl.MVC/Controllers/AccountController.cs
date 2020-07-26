using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinyUrl.DAL.Models;
using TinyUrl.DAL;
using TinyUrl.Logic.Services;
using TinyURl.MVC.Models.ViewModels;

namespace TinyURl.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly MakeTinyUrlService _tinyUrlService;

        public AccountController(MakeTinyUrlService tinyUrlService) => _tinyUrlService = tinyUrlService;
        
        [HttpGet]
        public async Task<IActionResult> LoginStranger()
        {
            var model = new LoginModel{Name = "Anonimus",Password = "0000"};
            await Authenticate(model.Name);
            return View();
        }
        
        [HttpGet]
        public IActionResult Login() => View();
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _tinyUrlService.FindUserOrNullAsyncByCondition
                    (u => u.UserName == model.Name && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Name); 
                    return RedirectToAction("Index", "TinyUrl");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Register()=> View();
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _tinyUrlService.FindUserOrNullAsyncByCondition(u => u.UserName == model.Name);
                if (user == null)
                {
                    _tinyUrlService.AddUser(new User { UserName = model.Name, Password = model.Password });
                    await Authenticate(model.Name); 
                    return RedirectToAction("Index", "TinyUrl");
                }
                else
                    ModelState.AddModelError("", "Имя пользователя уже существует. Выберите другое.");
            }
            return View(model);
        }
        
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        
    }
}