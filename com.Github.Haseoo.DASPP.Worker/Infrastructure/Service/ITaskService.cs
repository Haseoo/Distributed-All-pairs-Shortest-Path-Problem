using com.Github.Haseoo.DASPP.CoreData.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Worker.Infrastructure.Service
{
    public interface IWorkerService
    {
        Guid AddGraph(GraphDto graph);
        void RemoveGraph(Guid graphId);
    }
}
