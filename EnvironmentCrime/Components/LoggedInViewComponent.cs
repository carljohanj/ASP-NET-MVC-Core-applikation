using Microsoft.AspNetCore.Mvc;
using EnvironmentCrime.Models;

namespace EnvironmentCrime.Components
{
    public class LoggedInViewComponent : ViewComponent
    {
        private readonly IRepository repository;

        public LoggedInViewComponent(IRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke()
        {

            ViewBag.User = "";

            if (User.IsInRole("Coordinator"))
            {
                ViewBag.User = "samordnare";
            }
            else if(User.IsInRole("Manager"))
            {
                ViewBag.User = "avdelningschef";
            }
            else if (User.IsInRole("Investigator"))
            {
                ViewBag.User = "handläggare";
            }

            return View("LoggedIn");
        }


    }
}
