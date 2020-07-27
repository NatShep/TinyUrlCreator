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

        public TinyUrlController(MakeTinyUrlService tinyUrlService) => _tinyUrlService = tinyUrlService;
        
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
            //Check link. Is it Working?
            var ping = KnockToUrl.Knock(url.OriginalUrl);
            Task.WaitAll(ping);
            
            // Link is working
            if (ping.Result)
            {
                var user = await GetUserAsync(User.Identity.Name);
                //check user
                if (User.Identity.Name == "Anonimus")
                    return View(url);
                
                //check url. Is it Exist?
                var existingUrl = await _tinyUrlService.FindUrlOrNullByConditionAsync(u =>
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

                //CreateTinyUrl
                url.TinyPath = await _tinyUrlService.CreateTinyUrlForUserAsync(user, url.OriginalUrl);
                url.UrlExist = true;

                //Add TinyUrl
                await _tinyUrlService.AddUrlAsync(new Url
                {
                    OriginalPath = url.OriginalUrl,
                    TinyPath = url.TinyPath,
                    User = user,
                });
            }
            //Link is not working
            else
            {
                url.UrlExist = false;
                ModelState.AddModelError("", "Вы ввели не рабочую ссылку либо ссылка не отвечает. Попробуйте позднее.");
            }
            return View(url);
        }

        [HttpGet]
        [Authorize]
        public async  Task<IActionResult> History()
        {
            var user = await GetUserAsync(User.Identity.Name);
            var historyList = await _tinyUrlService.GetHistoryListForUserAsync(user);
            return View(historyList);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUrls()
        {
            var user = await GetUserAsync(User.Identity.Name);
            var userUrls = await _tinyUrlService.FindUrlsByUserAsync(user.Id);
            return View(userUrls);
        }

        [Route("TinyUrl/{tinyPath}")]
        [HttpGet]
        public async Task<IActionResult> GoToTinyUrl(string tinyPath)
        {
            //find tiny url with this tinyPath (tinyPath is unique)
            var url =await _tinyUrlService.FindUrlOrNullByConditionAsync(u => u.TinyPath == tinyPath);
            if (url == null)
            {
                ModelState.AddModelError("", "Переход по несуществующей ссылке.");
                return View();
            }
            
            //find the User(holder) of this tiny url
            var user = await _tinyUrlService.GetUserByUrlAsync(url.Id);
            
            //increase number of transition and history, if User or someone else use this tiny url
            //??   maybe better to update history when user(not everybody) go to url  ??
            // TODO add info(for example time) of transition
            var tinyUrl = await _tinyUrlService.IncreaseNumberOfTransitionByOneAsync(url);
             await _tinyUrlService.UpdateHistoryForUserAsync(url.TinyPath, user);

            return new RedirectResult(tinyUrl.OriginalPath);
        }
        private async Task<User> GetUserAsync(string name)
        {
            var user = await _tinyUrlService.FindUserOrNullByConditionAsync(u =>
                u.UserName == name);
            if (user == null)
                return new User {UserName = "Anonymous"};
            return user;
        }
    }
}