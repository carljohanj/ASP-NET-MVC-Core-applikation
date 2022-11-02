using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EnvironmentCrime.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly IRepository repository;
        private IHttpContextAccessor contextAcc;

        public ManagerController(IRepository repository, IHttpContextAccessor contextAcc)
        {
            this.repository = repository;
            this.contextAcc = contextAcc;
        }

        public ViewResult StartManager()
        {
            ViewBag.UserName = contextAcc.HttpContext.User.Identity.Name;
            string depID = repository.GetManagerDept(ViewBag.UserName);

            ViewBag.Departments = repository.Departments;
            ViewBag.DeptEmployees = repository.GetEmployeesForDepartment(depID);
            ViewBag.ErrandList = repository.ShowErrandListManager(ViewBag.UserName);

            return View();
        }

        public ViewResult CrimeManager(int id)
        {
            ViewBag.UserName = contextAcc.HttpContext.User.Identity.Name;
            TempData["ID"] = id;
            ViewBag.ID = id;

            string depID = repository.GetManagerDept(ViewBag.UserName);
            ViewBag.DeptEmployees = repository.GetEmployeesForDepartment(depID);

            return View();
        }


        //Helper method to save investigator choices to database:
        [HttpPost]
        public IActionResult AssignInvestigatorToErrand(Errand errand, bool noAction)
        {
            int ErrandId = int.Parse(TempData["ID"].ToString());
            
            repository.UpdateErrandWithInvestigator(ErrandId, errand, noAction);

            return RedirectToAction("StartManager");
        }

    }
}