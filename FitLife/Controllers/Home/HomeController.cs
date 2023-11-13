using System.Diagnostics;
using FirebaseAuth;
using FitLife.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Firebase.Auth.Providers;

namespace FitLife.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
        {
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
