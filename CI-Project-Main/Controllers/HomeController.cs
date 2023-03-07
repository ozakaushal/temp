using CI_Project_Main.Models;
using CI_Project_Main.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Project_Main.Controllers
{
    public class HomeController : Controller


    {
        private readonly CIPContext _db;

        public HomeController(CIPContext db)
        {
            _db = db;
        }

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