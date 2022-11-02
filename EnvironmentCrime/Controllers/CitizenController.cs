using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Mvc;
using EnvironmentCrime.Infrastructure;

namespace EnvironmentCrime.Controllers
{
    public class CitizenController : Controller
    {
        private readonly IRepository repository;

        public CitizenController(IRepository repo)
        {
            repository = repo;
        }
    
        public ViewResult Contact()
        {
            return View();
        }

        public ViewResult Faq()
        {
            return View();
        }

        public ViewResult Services()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Validate(Errand errand)
        {
            HttpContext.Session.Set<Errand>("NewErrand", errand);
            return View(errand);
        }


        public ViewResult Thanks()
        {
            var dbEntry = HttpContext.Session.Get<Errand>("NewErrand");
            repository.SaveErrand(dbEntry);
            ViewBag.RefNumber = dbEntry.RefNumber;
            HttpContext.Session.Remove("NewErrand");
            return View();
        }
    }
}