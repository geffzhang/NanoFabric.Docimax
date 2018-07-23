using Microsoft.AspNetCore.Mvc;

namespace NanoFabric.Docimax.AccountManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
