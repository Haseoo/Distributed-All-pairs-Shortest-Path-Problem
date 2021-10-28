using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Main.Dtos;
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

        public BestVertexResponseDto CalculateBestVertexDijkstra(GraphDto graphDto)
        {
            var stopWatch = Stopwatch.StartNew();

            var results = new List<Result>();

            var bestVertex = new Result(int.MaxValue, -1);
            for (var i = 0; i < graphDto.GraphSize; i++)
            {
                results.Add(new Result(Dijkstra(i, graphDto), i));
            }

            foreach (var result in
                from result in results
                where result.RoadCost < bestVertex.RoadCost
                select result)
            {
                bestVertex = result;
            }

            stopWatch.Stop();

            return new BestVertexResponseDto()
            {
                BestVertexIndex = bestVertex.Vertex,
                BestVertexRoadCost = bestVertex.RoadCost,
                CalculationTimeMs = stopWatch.ElapsedMilliseconds
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



        private int Dijkstra(int vertex, GraphDto graphDto)
        {
            var elements = new List<Element>();

            for (var i = 0; i < graphDto.GraphSize; i++)
            {
                elements.Add(new Element());
                elements[i].Distance = int.MaxValue;
                elements[i].Previous = -1;
                elements[i].IsCounted = false;
            }
            elements[vertex].Distance = 0;

            while (elements.Any(x => !x.IsCounted))
            {
                var u = FindMinDistanceElement(elements);

                elements[u].IsCounted = true;

                for (var i = 0; i < graphDto.GraphSize; i++)
                {
                    if (Adjacent(u, i, graphDto) && u != i)
                    {
                        if (elements[i].Distance > (Length(u, i, graphDto) + elements[u].Distance))
                        {
                            elements[i].Distance = Length(u, i, graphDto) + elements[u].Distance;
                            elements[i].Previous = u;
                        }
                    }
                }
            }

            return GetDistanceSum(elements);
        }

        private bool Adjacent(int vertexA, int vertexB, GraphDto graphDto)
        {
            return (graphDto[vertexA, vertexB] > 0);
        }

        private int Length(int vertexU, int vertexV, GraphDto graphDto)
        {
            return graphDto[vertexU, vertexV];
        }

        private int FindMinDistanceElement(IList<Element> elements)
        {
            var element = new Element() { Distance = int.MaxValue };
            var index = -1;

            foreach (var item in elements)
            {
                if (item.Distance < element.Distance && !item.IsCounted)
                {
                    element = item;
                    index = elements.IndexOf(item);
                }
            }
            return index;
        }

        private int GetDistanceSum(IEnumerable<Element> elements)
        {
            return elements.Sum(item => item.Distance);
        }
    }
}