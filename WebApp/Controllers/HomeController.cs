using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Options;

namespace WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppOptions _appOptions;

        public HomeController(ILogger<HomeController> logger, IOptions<AppOptions> appOptions)
        {
            _logger = logger;
            _appOptions = appOptions.Value;
        }

        public IActionResult Index()
        {
            ViewBag.AppName = _appOptions.AppName;
            ViewBag.Version = _appOptions.Version;
            ViewBag.Environment = _appOptions.Environment;
            ViewBag.BaseUrl = _appOptions.BaseUrl;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
