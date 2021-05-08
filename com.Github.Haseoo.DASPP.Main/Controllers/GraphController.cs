﻿using System.IO;
using System.Text;
using com.Github.Haseoo.DASPP.Main.Dtos;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace com.Github.Haseoo.DASPP.Main.Controllers
{
    [ApiController]
    [Route("/api/graph")]
    public class GraphController : ControllerBase
    {
        public GraphController(IGraphService graphService)
        {
            _graphService = graphService;
        }

        private readonly IGraphService _graphService;

        [HttpPut]
        public IActionResult CalculateBestVertex(MainTaskRequestDto request)
        {
            return Ok(_graphService.CalculateBestVertex(request));
        }
        [HttpPost]
        public FileStreamResult GenerateGraph(int numberOfNodes)
        {
            var stream = _graphService.GenerateGraph(numberOfNodes);
            return new FileStreamResult(
                stream,
                new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream"))
            {
                FileDownloadName = "generatedGraph.json"
            };
        }
    }
}