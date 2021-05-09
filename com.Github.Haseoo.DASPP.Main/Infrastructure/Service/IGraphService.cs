using com.Github.Haseoo.DASPP.Main.Dtos;
using System.IO;

namespace com.Github.Haseoo.DASPP.Main.Infrastructure.Service
{
    public interface IGraphService
    {
        MainTaskResponseDto CalculateBestVertex(MainTaskRequestDto request);

        Stream GenerateGraph(int numberOfNodes);
    }
}