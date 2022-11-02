using Microsoft.AspNetCore.Mvc;
using EnvironmentCrime.Models;
using EnvironmentCrime.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace EnvironmentCrime.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class CoordinatorController : Controller
    {
        private readonly IRepository repository;
        private IHttpContextAccessor contextAcc;

        public CoordinatorController(IRepository repo, IHttpContextAccessor contextAcc)
        {
            repository = repo;
            this.contextAcc = contextAcc;
        }

        public ViewResult StartCoordinator()
        {
            var tempErrand = HttpContext.Session.Get<Errand>("NewErrand");

            @ViewBag.UserName = contextAcc.HttpContext.User.Identity.Name;
            ViewBag.ErrandList = repository.ShowErrandListCoordinator();

            return View();
        }

        public ViewResult CrimeCoordinator(int id)
        {
            TempData["ID"] = id;
            ViewBag.ID = id;
            ViewBag.DepartmentList = repository.Departments;
            return View();
        }

        public ViewResult ReportCrime()
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


        //Helper method to save choice from dropdown menu to database:
        public IActionResult AssignDepartmentToErrand(Department department)
        {
            int ErrandId = int.Parse(TempData["ID"].ToString());

            repository.UpdateErrandWithDepartmentId(ErrandId, department.DepartmentId);

            return RedirectToAction("StartCoordinator");
        }

    }
}