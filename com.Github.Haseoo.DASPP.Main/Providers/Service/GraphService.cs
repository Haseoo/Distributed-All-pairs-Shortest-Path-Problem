using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Main.Dtos;
using com.Github.Haseoo.DASPP.Main.Exceptions.Workers;
using com.Github.Haseoo.DASPP.Main.Helper;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var averageCalculationTime = 0;
            var stopWatch = Stopwatch.StartNew();

            var helpers = _workerHostService.GetWorkers()
                .Select(e => new ClientHelper(e, request.GraphDto))
                .ToList();

            var begin = 0;
            var end = request.GraphDto.GraphSize - 1;
            var vertexCount = end - begin + 1;
            var workerCount = helpers.Count;

            if (workerCount == 0)
            {
                throw new NoWorkerPresentException();
            }

            var packageSize = request.Granulation;
            if (packageSize <= 0)
            {
                packageSize = (vertexCount / workerCount != 0) ? (vertexCount / workerCount) : 1;
            }

            var bestResult = new ResultDto()
            {
                Vertex = -1,
                RoadCost = int.MaxValue
            };

            while (begin <= end)
            {
                var tasks = new List<Task<ResultDto>>();
                for (var i = 0; i < workerCount && begin <= end; i++)
                {
                    var task = begin + packageSize > end ? helpers[i].CalculateFor(begin, end)
                        : helpers[i].CalculateFor(begin, begin + packageSize);
                    begin += packageSize + 1;
                    tasks.Add(task);
                }

                var results = Task.WhenAll(tasks.ToArray()).Result;
                averageCalculationTime += (int)results.Average(e => e.CalculatingTimeMs);
                var bestPartialResult = results.OrderBy(e => e.RoadCost).FirstOrDefault();
                if (bestPartialResult?.RoadCost < bestResult.RoadCost)
                {
                    bestResult = bestPartialResult;
                }
            }

            stopWatch.Stop();
            var totalTime = (int)stopWatch.Elapsed.TotalMilliseconds;
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
    }
}