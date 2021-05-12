using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace com.Github.Haseoo.DASPP.Main.Controllers
{
    [Route("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MainPageController : Controller
    {
        private readonly IWorkerHostService _workerHostService;

        public MainPageController(IWorkerHostService workerHostService)
        {
            _workerHostService = workerHostService;
        }

        public IActionResult Index()
        {
            return View("MainPage", _workerHostService.GetWorkers());
        }
    }
}