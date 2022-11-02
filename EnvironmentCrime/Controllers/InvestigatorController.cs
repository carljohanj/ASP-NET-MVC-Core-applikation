using EnvironmentCrime.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EnvironmentCrime.Controllers
{
    [Authorize(Roles = "Investigator")]
    public class InvestigatorController : Controller
    {
        private readonly IRepository repository;
        private IHttpContextAccessor contextAcc;

        public InvestigatorController(IRepository repository, IWebHostEnvironment environment, IHttpContextAccessor contextAcc)
        {
            this.repository = repository;
            this.contextAcc = contextAcc;
        }

        public ViewResult StartInvestigator()
        {
            ViewBag.UserName = contextAcc.HttpContext.User.Identity.Name;

            ViewBag.ErrandStatusList = repository.ErrandStatuses;
            ViewBag.ErrandList = repository.ShowErrandListInvestigator(ViewBag.UserName);

            return View();
        }

        public ViewResult CrimeInvestigator(int id)
        {
            TempData["ID"] = id;
            ViewBag.ID = id;
            ViewBag.ErrandStatusList = repository.ErrandStatuses;

            return View();
        }


        //Helper method to save investigator choices to database:
        [HttpPost]
        public async Task<IActionResult> InvestigatorActions(Errand errand, IFormFile loadsample, IFormFile loadimage)
        {
            int ErrandId = int.Parse(TempData["ID"].ToString());

            if (loadimage != null)
            {
                await repository.SaveFile(ErrandId, loadimage, "image");
            }

            if (loadsample != null)
            {
               await repository.SaveFile(ErrandId, loadsample, "sample");
            }

            if(errand.StatusId != "Välj" || errand.InvestigatorAction != null || errand.InvestigatorInfo != null)
            {
                repository.UpdateErrandWithInvestigatorActions(ErrandId, errand);
            }

            return RedirectToAction("StartInvestigator");
        }

    }
}
