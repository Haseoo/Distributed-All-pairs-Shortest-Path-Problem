using com.Github.Haseoo.DASPP.Main.Dtos;
using System.IO;
using com.Github.Haseoo.DASPP.CoreData.Dtos;

namespace com.Github.Haseoo.DASPP.Main.Infrastructure.Service
{
    public interface IGraphService
    {
        BestVertexResponseDto CalculateBestVertexDijkstra(GraphDto graphDto);
        BestVertexResponseDto CalculateBestVertexFloydWarshall(GraphDto graphDto);

        Stream GenerateGraph(int numberOfNodes);
    }
}