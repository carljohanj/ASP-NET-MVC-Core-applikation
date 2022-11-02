using Microsoft.AspNetCore.Mvc;

namespace EnvironmentCrime.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
