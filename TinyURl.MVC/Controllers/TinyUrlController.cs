using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
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

                if (_tinyUrlService.FindSomeUrlsByCondition
                    (u => u.OriginalPath == url.OriginalUrl && u.User == user).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Эта ссылка уже добавлена");
                    return View(url);
                }

                url.TinyPath = _tinyUrlService.CreateTinyUrlForUser(user, url.OriginalUrl);
                url.UrlExist = true;

                var uRl = new Url
                {
                    OriginalPath = url.OriginalUrl,
                    TinyPath = url.TinyPath, 
                    User=user,
                }; 
                _tinyUrlService.AddUrl(uRl);
            }
            else
                url.UrlExist = false;

            return View(url);
        }

        [HttpGet]
        [Authorize]
        public IActionResult History()
        {
            var user = GetUser(User.Identity.Name);
            var userHistory = user.History;
            var historyList=new List<Url>();
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
        [Authorize]
        public IActionResult GoToTinyUrl(string tinyPath)
        {
            var url = _tinyUrlService.FindFirsUrlOrNullByCondition(u => u.TinyPath == tinyPath);
            var user = GetUser(User.Identity.Name);
            var tinyUrl = _tinyUrlService.FindUrlAndIncreaseNumberOfTransitionByOne(url);
            _tinyUrlService.UpdateHistoryForUser(url.TinyPath, user);
            return new RedirectResult(tinyUrl.OriginalPath);
        }

        private User GetUser(string name)
        {
            return _tinyUrlService.FindUserOrNullByCondition(u =>
                u.UserName == name);
        }
    }
}