using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Mvc;
using EnvironmentCrime.Infrastructure;

namespace EnvironmentCrime.Controllers
{
    public class HomeController : Controller
    {

        private readonly IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult Index()
        {
            var tempErrand = HttpContext.Session.Get<Errand>("NewErrand");

            if(tempErrand == null)
            {
                return View();
            }
            else
            {
                return View(tempErrand);
            }
        }
        
        public ViewResult Login()
        {
            return View();
        }        
    }
}
