using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using com.Github.Haseoo.DASPP.Main.Security;
using com.Github.Haseoo.DASPP.CoreData.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace com.Github.Haseoo.DASPP.Main.Controllers
{
    [ApiController]
    [Route("/api/worker")]
    public class WorkerHostController : Controller
    {
        private readonly IWorkerHostService _iWorkerHostService;

        public WorkerHostController(IWorkerHostService iWorkerHostService)
        {
            _iWorkerHostService = iWorkerHostService;
        }

        [HttpGet]
        public IActionResult GetRegisteredWorkerHosts()
        {
            return Ok(_iWorkerHostService.GetWorkers());
        }

        [HttpPost]
        [ProtectedByApiKey]
        public IActionResult RegisterWorker(WorkerHostInfo info)
        {
            _iWorkerHostService.RegisterWorker(info);
            return NoContent();
        }

        [HttpDelete]
        [ProtectedByApiKey]
        public IActionResult DeRegisterWorker(string uri)
        {
            _iWorkerHostService.DeregisterWorker(uri);
            return NoContent();
        }
    }
}