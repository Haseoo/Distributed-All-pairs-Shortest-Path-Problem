using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Main.Dtos;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
        public IActionResult CalculateBestVertex(FindBestVertexRequestDto request)
        {
            return Ok(request.Algorithm == Algorithm.Dijkstra ? _graphService.CalculateBestVertexDijkstra(request.GraphDto) : null);
        }

        [HttpGet("generating")]
        public FileStreamResult GenerateGraph(int numberOfNodes)
        {
            var stream = _graphService.GenerateGraph(numberOfNodes);
            return new FileStreamResult(
                stream,
                new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = "generatedGraph.json"
            };
        }
    }
}