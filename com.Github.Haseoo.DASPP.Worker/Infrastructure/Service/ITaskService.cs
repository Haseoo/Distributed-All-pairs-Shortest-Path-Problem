using com.Github.Haseoo.DASPP.CoreData.Dtos;
using System;

namespace com.Github.Haseoo.DASPP.Worker.Infrastructure.Service
{
    public interface ITaskService
    {
        Guid StartTask(GraphDto graph);

        void RemoveTask(Guid graphId);
    }
}