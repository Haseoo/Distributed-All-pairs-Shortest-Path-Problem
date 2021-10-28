using com.Github.Haseoo.DASPP.CoreData.Dtos;
using System;

namespace com.Github.Haseoo.DASPP.Main.Infrastructure.Service
{
    public interface ITaskService
    {
        Guid StartTask(GraphDto graph);

        ResultDto FindBestVertex(Guid graphId, int begin, int end);

        void RemoveTask(Guid graphId);
    }
}