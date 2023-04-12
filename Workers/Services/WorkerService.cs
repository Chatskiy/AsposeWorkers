using System.Collections.Generic;
using Workers.Models.Interfaces;
using Workers.Services.Interfaces;

namespace Workers.Services
{
    
    public class WorkerService : IWorkerService
    {
        /// <summary>
        /// Workers storage
        /// </summary>
        private readonly IWorkersStorage _workersStorage;

        public WorkerService(IWorkersStorage workers)
        {
            _workersStorage = workers;
        }

        /// <inheritdoc />
        public IWorker Add(IWorker worker)
        {
            return _workersStorage.Add(worker);
        }

        /// <inheritdoc />
        public IWorker Get(uint id)
        {
            return _workersStorage.Get(id);
        }

        /// <inheritdoc />
        public IEnumerable<IWorker> Get()
        {
            return _workersStorage.Get();
        }

        /// <inheritdoc />
        public void Remove(uint id)
        {
            _workersStorage.Remove(id);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _workersStorage.Clear();
        }
    }
}
