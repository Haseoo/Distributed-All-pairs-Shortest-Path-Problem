using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Main.Exceptions.Workers;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace com.Github.Haseoo.DASPP.Main.Providers.Service
{
    public class WorkerHostService : IWorkerHostService
    {
        private readonly IDictionary<string, WorkerHostInfo> _registeredWorkers = new ConcurrentDictionary<string, WorkerHostInfo>();
        private readonly ILogger<WorkerHostService> _logger;

        public WorkerHostService(ILogger<WorkerHostService> logger)
        {
            _logger = logger;
        }

        public void DeregisterWorker(string uri)
        {
            if (!_registeredWorkers.ContainsKey(uri))
            {
                throw new WorkerDoesNotExistException(uri);
            }
            _registeredWorkers.Remove(uri);
            _logger.LogInformation($"Deregistered {uri} worker");
        }

        public IEnumerable<WorkerHostInfo> GetWorkers()
        {
            return new List<WorkerHostInfo>(_registeredWorkers.Values);
        }

        public void RegisterWorker(WorkerHostInfo info)
        {
            if (_registeredWorkers.ContainsKey(info.Uri))
            {
                throw new WorkerAlreadyRegisteredException(info.Uri);
            }
            _registeredWorkers.Add(info.Uri, info);
            _logger.LogInformation($"Registered {info.Uri} worker");
        }
    }
}