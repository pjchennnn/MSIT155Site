using Microsoft.AspNetCore.Mvc;

namespace MSIT155Site.Controllers
{
    public class HomeWorkController : Controller
    {
        private readonly ILogger<HomeWorkController> _logger;

        public HomeWorkController(ILogger<HomeWorkController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult trav() { return View(); }

        public IActionResult Register()
        {
            return View();
        }
    }
}
