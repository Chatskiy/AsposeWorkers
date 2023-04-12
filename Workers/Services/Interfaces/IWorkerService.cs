using System.Collections.Generic;
using Workers.Models.Interfaces;

namespace Workers.Services.Interfaces
{

    /// <summary>
    /// Implements a worker service
    /// </summary>
    public interface IWorkerService
    {
        /// <summary>
        /// Add worker to storage
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        IWorker Add(IWorker worker);

        /// <summary>
        /// Get worker by Id
        /// </summary>
        /// <param name="id">identifier</param>
        /// <returns></returns>
        IWorker Get(uint id);

        /// <summary>
        /// Get all workers
        /// </summary>
        /// <returns></returns>
        IEnumerable<IWorker> Get();

        /// <summary>
        /// Remove worker by Id
        /// </summary>
        /// <param name="id"></param>
        void Remove(uint id);

        /// <summary>
        /// Remove all workers
        /// </summary>
        void Clear();
    }
}
