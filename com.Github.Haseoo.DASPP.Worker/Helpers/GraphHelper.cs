using com.Github.Haseoo.DASPP.CoreData.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Worker.Helpers
{
    public class GraphHelper
    {
        private readonly GraphDto _graphDto;
        private readonly int _graphSize;
        public Guid Id { get; private set; }

        private class Element
        {
            public int Distance { get; set; }
            public bool IsCounted { get; set; }
            public int Previous { get; set; }
        }


        public GraphHelper(GraphDto graphDto)
        {
            _graphDto = graphDto;
            _graphSize = _graphDto.GraphSize;
            Id = Guid.NewGuid();
        }

        public ResultDto FindShortestPathVertex(List<int> vertices)
        {
            var results = new List<ResultDto>();

            ResultDto bestVertex = new ResultDto(int.MaxValue, -1);
            foreach (var item in vertices)
            {
                results.Add(new ResultDto(Dijkstra(item), item));
            }

            //Console.WriteLine("All results: ");
            foreach (var result in results)
            {
                // Console.WriteLine($"Vertex: {result.Vertex}, road: {result.RoadCost}.");
                if (result.RoadCost < bestVertex.RoadCost)
                    bestVertex = result;
            }
            return bestVertex;
        }

        private int Dijkstra(int vertex)
        {
            List<Element> elements = new List<Element>();

            for (int i = 0; i < _graphSize; i++)
            {
                elements.Add(new Element());
                elements[i].Distance = int.MaxValue;
                elements[i].Previous = -1;
                elements[i].IsCounted = false;
            }
            elements[vertex].Distance = 0;


            while (elements.Any(x => x.IsCounted == false))
            {
                var u = FindMinDistanceElement(elements);

                elements[u].IsCounted = true;

                for (int i = 0; i < _graphSize; i++)
                {
                    if (Adjacent(u, i) && u != i)
                    {
                        if (elements[i].Distance > (Length(u, i) + elements[u].Distance))
                        {
                            elements[i].Distance = Length(u, i) + elements[u].Distance;
                            elements[i].Previous = u;
                        }
                    }
                }
            }

            /*for (int i = 0; i < _graphSize; i++)
            {
                Console.WriteLine($"{i}: d: {elements[i].distance}, p: {elements[i].previous}");
            }

            PrintResultRoad(elements);*/
            return GetDistanceSum(elements);
        }

        private bool Adjacent(int vertexA, int vertexB)
        {
            return (_graphDto[vertexA, vertexB] > 0);
        }
        private int Length(int vertex_u, int vertex_v)
        {
            return _graphDto[vertex_u, vertex_v];
        }

        private void PrintResultRoad(List<Element> elements)
        {
            for (int i = 0; i < _graphSize; i++)
            {
                Stack<int> road = new Stack<int>();
                Console.Write($"{i}: ");
                for (int j = i; j > -1; j = elements[j].Previous)
                {
                    road.Push(j);
                }
            }
        }
        private int FindMinDistanceElement(List<Element> elements)
        {
            var element = new Element() { Distance = int.MaxValue };
            var index = -1;

            foreach (var item in elements)
            {
                if (item.Distance < element.Distance && item.IsCounted == false)
                {
                    element = item;
                    index = elements.IndexOf(item);
                }
            }
            return index;
        }

        private int GetDistanceSum(List<Element> elements)
        {
            var sum = 0;

            foreach (var item in elements)
            {
                sum += item.Distance;
            }

            return sum;
        }
    }
}
