using System.Collections.Generic;
using System.Linq;
using Workers.Models.Interfaces;

namespace Workers.Helpers
{
    /// <summary>
    /// Class for extention any worker action
    /// </summary>
    public static class WorkerHelper
    {
        /// <summary>
        /// Return all subordinates of all nesting levels
        /// </summary>
        /// <param name="worker">head worker</param>
        /// <returns>All subordinates</returns>
        public static IEnumerable<IWorker> GetAllGenerationsOfSubordinates(this ITopWorker worker)
        {
            foreach (var subordinate in worker.Subordinates)
            {
                var topWorker = subordinate as ITopWorker;
                if (topWorker != null && topWorker.Subordinates.Any())
                {
                    yield return topWorker;
                    foreach (var sub in GetAllGenerationsOfSubordinates(topWorker))
                    {
                        yield return sub;
                    } 
                }
                else
                {
                    yield return subordinate;
                }
            }
        }
    }
}
