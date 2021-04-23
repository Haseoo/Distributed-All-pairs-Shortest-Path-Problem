using com.Github.Haseoo.DASPP.CoreData.Dtos;
using System.Collections.Generic;

namespace com.Github.Haseoo.DASPP.Main.Infrastructure.Service
{
    public interface IWorkerHostService
    {
        void RegisterWorker(WorkerHostInfo info);

        void DeregisterWorker(string uri);

        IEnumerable<WorkerHostInfo> GetWorkers();
    }
}