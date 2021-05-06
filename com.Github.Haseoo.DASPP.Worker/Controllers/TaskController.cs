using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Worker.Exceptions;
using com.Github.Haseoo.DASPP.Worker.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace com.Github.Haseoo.DASPP.Worker.Controllers
{
    [ApiController]
    [Route("/api/task")]
    public class TaskController : Controller
    {
        private const string CookieKey = "sessionId";
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            this._taskService = taskService;
        }

        [HttpPost]
        public IActionResult BeginTask(GraphDto graph)
        {
            if (HttpContext.Request.Cookies.ContainsKey(CookieKey))
            {
                throw new SessionAlreadyExistsException();
            }
            var guid = _taskService.StartTask(graph);
            HttpContext.Response.Cookies.Append(CookieKey, guid.ToString(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return NoContent();
        }

        [HttpPut]
        //TODO add validator!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public IActionResult FindBestVertex(FindBestVertexRequestDto request)
        {
            if (!HttpContext.Request.Cookies.TryGetValue(CookieKey, out var cookie))
            {
                throw new SessionNotStartedException();
            }
            if (!Guid.TryParse(cookie, out var guid))
            {
                return BadRequest(new ErrorResponse("Invalid session id"));
            }

            return Ok(_taskService.FindBestVertex(guid, request.BeginVertexIndex, request.EndVertexIndex));
        }

        [HttpDelete]
        public IActionResult CancelTask()
        {
            if (!HttpContext.Request.Cookies.TryGetValue(CookieKey, out var cookie))
            {
                throw new SessionNotStartedException();
            }
            if (!Guid.TryParse(cookie, out var guid))
            {
                return BadRequest(new ErrorResponse("Invalid session id"));
            }
            _taskService.RemoveTask(guid);
            HttpContext.Response.Cookies.Delete(CookieKey);
            return NoContent();
        }
    }
}