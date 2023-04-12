using System.Collections.Generic;

namespace Workers.Models.Interfaces
{
    public interface ITopWorker : IWorker
    {
        /// <summary>
        /// Subordinates
        /// </summary>
        IEnumerable<IWorker> Subordinates { get; }

        /// <summary>
        /// Add subordinate
        /// </summary>
        /// <param name="subordinate">Подчиненный</param>
        void AddSubordinate(IWorker subordinate);

        /// <summary>
        /// Add subordinates
        /// </summary>
        /// <param name="subordinates"></param>
        void AddSubordinate(IEnumerable<IWorker> subordinates);

        /// <summary>
        /// Remove subordinate
        /// </summary>
        /// <param name="subordinate"></param>
        void RemoveSubordinate(IWorker subordinate);


        /// <summary>
        /// Remove subordinates
        /// </summary>
        void RemoveSubordinate(IEnumerable<IWorker> subordinates);

        /// <summary>
        /// Remove all subordinates
        /// </summary>
        void ClearSubordinates();
    }
}
