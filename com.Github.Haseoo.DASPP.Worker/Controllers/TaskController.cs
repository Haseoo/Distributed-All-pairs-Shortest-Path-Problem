using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Worker.Exceptions;
using com.Github.Haseoo.DASPP.Worker.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Worker.Controllers
{
    [ApiController]
    [Route("/api/graph")]
    public class TaskController : Controller
    {
        private readonly ITaskService workerService;

        public IActionResult BeginTask(GraphDto graph)
        {
            var guid = workerService.AddGraph(graph);
            HttpContext.Response.Cookies.Append("sessionId", guid.ToString(), new CookieOptions() { HttpOnly = true });
            return NoContent();
        }
        public IActionResult CancelTask()
        {
            if (!HttpContext.Request.Cookies.TryGetValue("sessionId", out var cookie))
            {
                throw new SessionNotStarted("Session has not been started");
            }
            if (Guid.TryParse(cookie, out var guid))
            {
                throw new ArgumentException("Invalid session id");
            }
            workerService.RemoveGraph(guid);
            return NoContent();
        }
    }
}
