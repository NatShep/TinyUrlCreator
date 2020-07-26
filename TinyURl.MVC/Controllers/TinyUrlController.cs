using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.DAL.Models;
using TinyUrl.Logic;
using TinyUrl.Logic.Services;
using TinyURl.MVC.Models.ViewModels;

namespace TinyURl.MVC.Controllers
{
    public class TinyUrlController : Controller
    {
        private readonly MakeTinyUrlService _tinyUrlService;

        public TinyUrlController(MakeTinyUrlService tinyUrlService)
        {
            _tinyUrlService = tinyUrlService;
        }

        [HttpGet]
        [Authorize]
        //[AllowAnonymous]
        public IActionResult Index() => View();

        [HttpGet]
        [Authorize]
        public IActionResult CreateTinyUrl() => View(new UrlModel());

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTinyUrl(UrlModel url)
        {
            var ping = KnockToUrl.Knock(url.OriginalUrl);
            Task.WaitAll(ping);
            if (ping.Result)
            {
                var user = GetUser(User.Identity.Name);
                if (User.Identity.Name == "Anonimus")
                {
                    url.TinyPath = _tinyUrlService.CreateTinyUrlForUser(user, url.OriginalUrl);
                    url.UrlExist = true;
                    return View(url);
                }

                var existingUrl =
                    _tinyUrlService.FindFirsUrlOrNullByCondition(u =>
                        u.OriginalPath == url.OriginalUrl && u.User == user);
                if (existingUrl != null)
                {
                    ModelState.AddModelError("", "Эта ссылка уже добавлена");
                    return View(new UrlModel
                    {
                        OriginalUrl = existingUrl.OriginalPath,
                        TinyPath = existingUrl.TinyPath,
                        UrlExist = true
                    });
                }

                url.TinyPath = _tinyUrlService.CreateTinyUrlForUser(user, url.OriginalUrl);
                url.UrlExist = true;

                _tinyUrlService.AddUrl(new Url
                {
                    OriginalPath = url.OriginalUrl,
                    TinyPath = url.TinyPath,
                    User = user,
                });
            }
            else
            {
                url.UrlExist = false;
                ModelState.AddModelError("", "Вы ввели не рабочую ссылку");
            }
            return View(url);
        }

        [HttpGet]
        [Authorize]
        public IActionResult History()
        {
            var user = GetUser(User.Identity.Name);
            var userHistory = user.History;
            var historyList = new List<Url>();
            foreach (var tinyUrl in userHistory)
            {
                var url = _tinyUrlService.FindFirsUrlOrNullByCondition(u => u.TinyPath == tinyUrl);
                if (url == null)
                {
                    throw new Exception("Find null links in userHistory");
                }

                historyList.Add(url);
            }

            return View(historyList);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllUrls()
        {
            var user = GetUser(User.Identity.Name);
            var userUrls = _tinyUrlService.FindUrlsByUser(user.Id);
            return View(userUrls);
        }

        [Route("TinyUrl/{tinyPath}")]
        [HttpGet]
        public IActionResult GoToTinyUrl(string tinyPath)
        {
            
            //find tiny url with this tinyPath (tinyPath is unique)
            var url = _tinyUrlService.FindFirsUrlOrNullByCondition(u => u.TinyPath == tinyPath);
            if (url == null)
            {
                ModelState.AddModelError("", "Переход по несуществующей ссылке.");
                return View();
            }
            
            //find the User(holder) of this tiny url
            var user = _tinyUrlService.GetUserByUrl(url.Id);
            
            //increase number of transition and history, if User or someone else use this tiny url
            //??   maybe better to update history when user(not everybody) go to url  ??
            // TODO add info(for example time) of transition
            var tinyUrl = _tinyUrlService.FindUrlAndIncreaseNumberOfTransitionByOne(url);
            _tinyUrlService.UpdateHistoryForUser(url.TinyPath, user);

            return new RedirectResult(tinyUrl.OriginalPath);
        }

        private User GetUser(string name)
        {
            var user = _tinyUrlService.FindUserOrNullByCondition(u =>
                u.UserName == name);
            if (user == null)
                return new User
                {
                    UserName = "Анонимный пользователь"
                };
            return user;
        }
        
        

    }
}