using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.Logic;

namespace TinyURl.MVC.Controllers
{
    public class TinyUrlController : Controller
    {

        [HttpGet]
        [Authorize]
        //    [AllowAnonymous]
        public IActionResult Index() => View();
        
        [HttpGet]
        [Authorize]
        public IActionResult CreateTinyUrl() => View();
        
        [HttpPost]
        [Authorize]
        public IActionResult CreateTinyUrl(string urlString)
        {
            var ping =KnockToUrl.Knock(urlString);
            Task.WaitAll(ping);
            if (ping.Result)
            {
                ViewBag.UrlString = urlString;
                ViewBag.TinyUrl = TinyCreator.CreateTinyUrl(urlString);
            }
            else
            {
                ViewBag.UrlString = "Нерабочая ссылка :(";
                ViewBag.TinyUrl = "Не возможно создать короткую ссылку";
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult History() => View();
        
        [HttpGet]
        [Authorize]
        public IActionResult GetAllUrls() => View();

        [HttpGet]
        [Authorize]
        public RedirectResult GoToTinyUrl(int? id)
        {
         return new RedirectResult("http://www.yandex.com");
        }

  

    }
}