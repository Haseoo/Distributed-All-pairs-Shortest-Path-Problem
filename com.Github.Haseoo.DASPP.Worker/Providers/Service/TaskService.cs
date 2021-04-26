using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Worker.Controllers;
using com.Github.Haseoo.DASPP.Worker.Exceptions;
using com.Github.Haseoo.DASPP.Worker.Helpers;
using com.Github.Haseoo.DASPP.Worker.Infrastructure.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.Github.Haseoo.DASPP.Worker.Providers.Service
{
    public class TaskService : ITaskService
    {
        private readonly IDictionary<Guid, GraphHelper> _graphHelpers = new ConcurrentDictionary<Guid, GraphHelper>();

        public Guid StartTask(GraphDto graph)
        {
            var helper = new GraphHelper(graph);
            if (_graphHelpers.ContainsKey(helper.Id))
            {
                throw new SessionAlreadyExists();
            }
            _graphHelpers.TryAdd(helper.Id, helper);
            return helper.Id;
        }

        public void RemoveTask(Guid graphGuid)
        {
            if (!_graphHelpers.ContainsKey(graphGuid))
            {
                throw new SessionNotStarted();
            }
            _graphHelpers.Remove(graphGuid);
        }
    }
}
