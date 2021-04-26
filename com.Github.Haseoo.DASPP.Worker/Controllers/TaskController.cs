using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Worker.Exceptions;
using com.Github.Haseoo.DASPP.Worker.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace com.Github.Haseoo.DASPP.Worker.Controllers
{
    [ApiController]
    [Route("/api/graph")]
    public class TaskController : Controller
    {
        private const string CookieKey = "sessionId";
        private readonly ITaskService _iTaskService;

        public TaskController(ITaskService iTaskService)
        {
            this._iTaskService = iTaskService;
        }

        [HttpPost]
        public IActionResult BeginTask(GraphDto graph)
        {
            if (HttpContext.Request.Cookies.ContainsKey(CookieKey))
            {
                throw new SessionAlreadyExists();
            }
            var guid = _iTaskService.StartTask(graph);
            HttpContext.Response.Cookies.Append(CookieKey, guid.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return NoContent();
        }

        [HttpDelete]
        public IActionResult CancelTask()
        {
            if (!HttpContext.Request.Cookies.TryGetValue(CookieKey, out var cookie))
            {
                throw new SessionNotStarted();
            }
            if (!Guid.TryParse(cookie, out var guid))
            {
                throw new ArgumentException("Invalid session id");
            }
            _iTaskService.RemoveTask(guid);
            HttpContext.Response.Cookies.Delete(CookieKey);
            return NoContent();
        }
    }
}