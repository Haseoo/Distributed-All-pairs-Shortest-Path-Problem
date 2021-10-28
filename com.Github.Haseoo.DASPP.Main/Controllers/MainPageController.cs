using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace com.Github.Haseoo.DASPP.Main.Controllers
{
    [Route("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MainPageController : Controller
    {

        public IActionResult Index()
        {
            return View("MainPage");
        }
    }
}