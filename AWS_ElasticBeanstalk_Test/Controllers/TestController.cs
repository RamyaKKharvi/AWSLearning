using AWS_ElasticBeanstalk_Test.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AWS_ElasticBeanstalk_Test.Controllers
{
    public class TestController(ILogger<TestController> logger, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly ILogger<TestController> _logger = logger;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        public IActionResult Demo()
        {
            _logger.LogInformation("Home-Demo: Process started...");
            ViewBag.env = _webHostEnvironment.EnvironmentName;
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Privacy: Process started...");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
