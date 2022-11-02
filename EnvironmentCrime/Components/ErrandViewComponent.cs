using Microsoft.AspNetCore.Mvc;
using EnvironmentCrime.Models;

namespace EnvironmentCrime.Components
{
    public class ErrandViewComponent : ViewComponent
    {

        private readonly IRepository repository;

        public ErrandViewComponent(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var errandInfo = await repository.GetErrandInfo(id);
            return View("Errand", errandInfo);
        }

    }
}