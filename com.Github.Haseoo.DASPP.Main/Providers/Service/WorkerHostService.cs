using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using com.Github.Haseoo.DASPP.Worker.CoreData.Dtos;
using Microsoft.Extensions.Logging;
using System;
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
                throw new Exception("TODO worker key does not exist"); //TODO
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
            _registeredWorkers.Add(info.Uri, info);
            _logger.LogInformation($"Registered {info.Uri} worker");
        }
    }
}