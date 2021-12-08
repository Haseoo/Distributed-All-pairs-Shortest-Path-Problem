using System;
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

        public BestVertexResponseDto CalculateBestVertexFloydWarshall(GraphDto graphDto)
        {
            var stopWatch = Stopwatch.StartNew();
            var resultMatrix = ResultWarshallMatrix(WarshallFirstMatrix(graphDto), graphDto);
            var results = new List<Result>();
            var bestVertex = new Result(int.MaxValue, -1);
            for (var i = 0; i < graphDto.GraphSize; i++)
            {
                var sum = 0;
                for (var j = 0; j < graphDto.GraphSize; j++)
                {
                    sum += resultMatrix[i, j];
                }

                results.Add(new Result(sum, i));
            }

            foreach (var vertex in results.Where(vertex => vertex.RoadCost < bestVertex.RoadCost))
            {
                bestVertex = vertex;
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

        private static int Dijkstra(int vertex, GraphDto graphDto)
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

        private static bool Adjacent(int vertexA, int vertexB, GraphDto graphDto)
        {
            return (graphDto[vertexA, vertexB] > 0);
        }

        private static bool Adjacent(int vertexA, int vertexB, int[,] matrix)
        {
            return matrix[vertexA, vertexB] > 0 && matrix[vertexA, vertexB] != Int32.MaxValue;
        }

        private static int Length(int vertexU, int vertexV, GraphDto graphDto)
        {
            return graphDto[vertexU, vertexV];
        }

        private static int Length(int vertexU, int vertexV, int[,] matrix)
        {
            return matrix[vertexU, vertexV];
        }

        private static int FindMinDistanceElement(IList<Element> elements)
        {
            var element = new Element() {Distance = int.MaxValue};
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

        private static int GetDistanceSum(IEnumerable<Element> elements)
        {
            return elements.Sum(item => item.Distance);
        }

        private static int[,] WarshallFirstMatrix(GraphDto graphDto)
        {
            var firstMatrix = new int[graphDto.GraphSize, graphDto.GraphSize];
            for (var i = 0; i < graphDto.GraphSize; i++)
            {
                for (var j = 0; j < graphDto.GraphSize; j++)
                {
                    if (i == j)
                    {
                        firstMatrix[i, j] = 0;
                    }
                    else
                    {
                        if (Adjacent(i, j, graphDto))
                        {
                            firstMatrix[i, j] = Length(i, j, graphDto);
                        }
                        else
                        {
                            firstMatrix[i, j] = int.MaxValue;
                        }
                    }
                }
            }
            return firstMatrix;
        }

        private static int[,] ResultWarshallMatrix(int[,] firstMatrix, GraphDto graphDto)
        {
            var resultMatrix = firstMatrix;
            for (var k = 0; k < graphDto.GraphSize; k++)
            {
                for (var i = 0; i < graphDto.GraphSize; i++)
                {
                    for (var j = 0; j < graphDto.GraphSize; j++)
                    {
                        if (i == k || j == k || i == j) continue;
                        if (FoundShorterPath(i, j, k, resultMatrix))
                        {
                            resultMatrix[i, j] = Length(i, k, resultMatrix) + Length(k, j, resultMatrix);
                        }
                    }
                }
            }
            return resultMatrix;
        }

        private static bool FoundShorterPath(int i, int j, int k, int[,] matrix)
        {
            if (!Adjacent(i, j, matrix))
            {
                return Adjacent(i, k, matrix) && Adjacent(j, k, matrix);
            }

            if (!Adjacent(i, k, matrix) || !Adjacent(j, k, matrix))
                return false;

            return Length(i, j, matrix) > Length(i, k, matrix) + Length(k, j, matrix);
        }
    }
}