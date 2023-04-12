using System;
using System.Collections.Generic;
using Workers.Models;
using Workers.Models.Interfaces;
using Workers.Services.Interfaces;

namespace Workers.Services
{

    /// <summary>
    /// This interface encapsulates data storage logic
    /// This can be a database, serialization into various formats, and other ways of storing/retrieving information.
    /// </summary>
    public class WorkersStorage : IWorkersStorage
    {
        /// <summary>
        /// Value of current identifier
        /// </summary>
        private uint _currentId = 1;

        /// <summary>
        /// Workers storage
        /// </summary>
        private readonly Dictionary<uint, IWorker> _workers = new Dictionary<uint, IWorker>();

        /// <inheritdoc />
        public IWorker Add(IWorker worker)
        {
            if (worker == null)
            {
                throw new ArgumentNullException();
            }

            if (worker.Id == 0  
                || !_workers.ContainsKey(worker.Id))
            {
                worker.Id = _currentId++;
                _workers.Add(worker.Id, worker);
            }
            return worker;
        }

        /// <inheritdoc />
        public IWorker Get(uint id)
        {
            IWorker worker;
            if (!_workers.TryGetValue(id, out worker))
            {
                worker = null;
            }
            return worker;
        }

        /// <inheritdoc />
        public IEnumerable<IWorker> Get()
        {
            return _workers.Values;
        }

        /// <inheritdoc />
        public void Remove(uint id)
        {
            if (_workers.ContainsKey(id))
            {
                _workers.Remove(id);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            _workers.Clear();
        }
    }
}
