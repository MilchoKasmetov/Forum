namespace Forum.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Forum.Services.Data;
    using Forum.Web.ViewModels;
    using Forum.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;

    public class HomeController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public HomeController(
            ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Categories =this.categoriesService.GetAll<IndexCategoryViewModel>(),
            };
            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
