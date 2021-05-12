using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Main.Dtos;
using com.Github.Haseoo.DASPP.Main.Exceptions.Workers;
using com.Github.Haseoo.DASPP.Main.Helpers;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Main.Providers.Service
{
    public class GraphService : IGraphService
    {
        private readonly IWorkerHostService _workerHostService;

        public GraphService(IWorkerHostService workerHostService)
        {
            _workerHostService = workerHostService;
        }

        public MainTaskResponseDto CalculateBestVertex(MainTaskRequestDto request)
        {
            var stopWatch = Stopwatch.StartNew();

            var helpersTasks = _workerHostService.GetWorkers()
                .Select(e => ClientHelper.AsyncNew(e, request.GraphDto))
                .ToArray();

            var helpers = Task.WhenAll(helpersTasks).Result;

            var begin = 0;
            var end = request.GraphDto.GraphSize - 1;
            var vertexCount = end - begin + 1;
            var workerCount = helpers.Length;

            if (workerCount == 0)
            {
                throw new NoWorkerPresentException();
            }

            var packageSize = request.Granulation;
            if (packageSize <= 0)
            {
                packageSize = (vertexCount / workerCount != 0) ? (vertexCount / workerCount) : 1;
            }

            var tasks = new List<Task<ResultDto>>(workerCount);
            var results = new List<ResultDto>();
            var calculationTimeForWorkers = new long[workerCount];

            for (var i = 0; i < workerCount && begin <= end; i++)
            {
                var task = begin + packageSize > end ? helpers[i].CalculateFor(begin, end)
                    : helpers[i].CalculateFor(begin, begin + packageSize);
                begin += packageSize + 1;
                tasks.Add(task);
            }

            while (begin <= end)
            {
                var readyTaskIndex = Task.WaitAny(tasks.ToArray());
                var result = tasks[readyTaskIndex].Result;
                results.Add(result);
                tasks[readyTaskIndex] = begin + packageSize > end ? helpers[readyTaskIndex].CalculateFor(begin, end)
                    : helpers[readyTaskIndex].CalculateFor(begin, begin + packageSize);
                begin += packageSize + 1;
                calculationTimeForWorkers[readyTaskIndex] += result.CalculatingTimeMs;
            }

            int index;
            while ((index = Task.WaitAny(tasks.ToArray())) != -1)
            {
                var result = tasks[index].Result;
                calculationTimeForWorkers[index] += result.CalculatingTimeMs;
                results.Add(result);
                tasks.RemoveAt(index);
            }

            var bestResult = results.OrderBy(e => e.RoadCost).FirstOrDefault();
            var averageCalculationTime = 0L;
            var workersWithCalculationTimeGreaterThanZero = calculationTimeForWorkers.Where(e => e > 0);
            if (workersWithCalculationTimeGreaterThanZero.Any())
            {
                averageCalculationTime = (long)calculationTimeForWorkers.Average();
            }

            stopWatch.Stop();
            var totalTime = stopWatch.ElapsedMilliseconds;
            foreach (var clientHelper in helpers)
            {
                clientHelper.FinalizeSession();
            }
            return new MainTaskResponseDto()
            {
                BestVertexIndex = bestResult.Vertex,
                BestVertexRoadCost = bestResult.RoadCost,
                CalculationTimeMs = averageCalculationTime,
                TotalTaskTimeMs = totalTime,
                CommunicationTimeMs = totalTime - averageCalculationTime
            };
        }

        public Stream GenerateGraph(int numberOfNodes)
        {
            var graphGenerator = new GraphGenerator(numberOfNodes, 100);
            var graph = graphGenerator.GenerateGraph();
            var jsonGraph = JsonSerializer.Serialize(graph);
            var byteArray = Encoding.ASCII.GetBytes(jsonGraph);
            var stream = new MemoryStream(byteArray);
            return stream;
        }
    }
}