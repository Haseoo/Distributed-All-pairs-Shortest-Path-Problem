using System;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using com.Github.Haseoo.DASPP.Worker.CoreData.Dtos;
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
        public IActionResult RegisterWorker(WorkerHostInfo info)
        {
            _iWorkerHostService.RegisterWorker(info);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeRegisterWorker(string uri)
        {
            _iWorkerHostService.DeregisterWorker(uri);
            return NoContent();
        }
    }
}