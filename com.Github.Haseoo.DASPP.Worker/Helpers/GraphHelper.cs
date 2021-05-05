using com.Github.Haseoo.DASPP.CoreData.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace com.Github.Haseoo.DASPP.Worker.Helpers
{
    public class GraphHelper
    {
        private readonly GraphDto _graphDto;

        public Guid Id { get; private set; }

        public int GraphSize { get; }

        public GraphHelper(GraphDto graphDto)
        {
            _graphDto = graphDto;
            GraphSize = _graphDto.GraphSize;
            Id = Guid.NewGuid();
        }

        public Result FindShortestPathVertex(IEnumerable<int> vertices)
        {
            var results = new List<Result>();

            var bestVertex = new Result(int.MaxValue, -1);
            results.AddRange(vertices.Select(item => new Result(Dijkstra(item), item)));
            foreach (var result in
            from result in results
            where result.RoadCost < bestVertex.RoadCost
            select result)
            {
                bestVertex = result;
            }

            return bestVertex;
        }

        private int Dijkstra(int vertex)
        {
            var elements = new List<Element>();

            for (var i = 0; i < GraphSize; i++)
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

                for (var i = 0; i < GraphSize; i++)
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

            return GetDistanceSum(elements);
        }

        private bool Adjacent(int vertexA, int vertexB)
        {
            return (_graphDto[vertexA, vertexB] > 0);
        }

        private int Length(int vertexU, int vertexV)
        {
            return _graphDto[vertexU, vertexV];
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