using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.CoreData.Exceptions;
using com.Github.Haseoo.DASPP.Worker.Exceptions;
using com.Github.Haseoo.DASPP.Worker.Helpers;
using com.Github.Haseoo.DASPP.Worker.Infrastructure.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Worker.Providers.Service
{
    public class TaskService : ITaskService
    {
        private readonly IDictionary<Guid, GraphHelper> _graphHelpers = new ConcurrentDictionary<Guid, GraphHelper>();

        public Guid StartTask(GraphDto graph)
        {
            var helper = new GraphHelper(graph);
            if (_graphHelpers.ContainsKey(helper.Id))
            {
                throw new SessionAlreadyExistsException();
            }
            _graphHelpers.TryAdd(helper.Id, helper);
            return helper.Id;
        }

        public void RemoveTask(Guid graphId)
        {
            if (!_graphHelpers.ContainsKey(graphId))
            {
                throw new SessionNotStartedException();
            }
            _graphHelpers.Remove(graphId);
        }

        public ResultDto FindBestVertex(Guid graphId, int begin, int end)
        {
            if (!_graphHelpers.ContainsKey(graphId))
            {
                throw new NotFoundException($"Graph with id: {graphId}");
            }

            var stopWatch = Stopwatch.StartNew();

            var helper = _graphHelpers[graphId];
            begin = (begin > helper.GraphSize) ? helper.GraphSize : begin;
            end = (end > helper.GraphSize) ? helper.GraphSize : end;
            var vertexCount = end - begin + 1;
            var coreCount = Environment.ProcessorCount;
            var bestVertex = new Result(int.MaxValue, -1);
            var packageSize = (vertexCount / coreCount != 0) ? (vertexCount / coreCount) : 1;
            while (begin <= end)
            {
                var tasks = new List<Task<Result>>();
                for (var i = 0; i < coreCount && begin <= end; i++)
                {
                    var task = FindPartialResult(helper, begin + packageSize > end
                        ? Enumerable.Range(begin, end - begin + 1)
                        : Enumerable.Range(begin, packageSize));
                    task.Start();
                    tasks.Add(task);
                    begin += packageSize;
                }

                var partialResult = Task.WhenAll(tasks.ToArray())
                    .Result
                    .OrderBy(e => e.RoadCost)
                    .FirstOrDefault();
                if (partialResult != null && bestVertex.RoadCost > partialResult.RoadCost)
                {
                    bestVertex = partialResult;
                }
            }

            stopWatch.Stop();

            return new ResultDto()
            {
                RoadCost = bestVertex.RoadCost,
                Vertex = bestVertex.Vertex,
                CalculatingTimeMs = stopWatch.ElapsedMilliseconds
            };
        }

        private static Task<Result> FindPartialResult(GraphHelper helper, IEnumerable<int> range)
        {
            return new Task<Result>(() => helper.FindShortestPathVertex(range));
        }
    }
}