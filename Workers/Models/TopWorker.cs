using System;
using System.Collections.Generic;
using System.Linq;
using Workers.Enums;
using Workers.Models.Interfaces;

namespace Workers.Models
{

    public abstract class TopWorker : Worker, ITopWorker
    {
        /// <summary>
        /// Subordinates
        /// </summary>
        private readonly List<IWorker> _subordinates;

        public TopWorker(string name, DateTime employmentDate, double salary, WorkerType workerType)
            : base(name, employmentDate, salary, workerType)
        {
            _subordinates = new List<IWorker>();
        }

        /// <inheritdoc />
        public virtual IEnumerable<IWorker> Subordinates
        {
            get
            {
                return _subordinates;
            }
        }

        /// <inheritdoc />
        public virtual void AddSubordinate(IWorker subordinate)
        {
            AddSubordinate(new List<IWorker>(1) { subordinate });
        }

        /// <inheritdoc />
        public virtual void AddSubordinate(IEnumerable<IWorker> subordinates)
        {
            var existsIds = _subordinates.Select(s => s.Id);
            var uniqueSubordinates = subordinates.Where(s => !existsIds.Contains(s.Id));
            foreach (var worker in uniqueSubordinates)
            {
                worker.Boss = this;
            }
            _subordinates.AddRange(uniqueSubordinates);
        }

        /// <inheritdoc />
        public virtual void RemoveSubordinate(IWorker subordinate)
        {
            RemoveSubordinate(new List<IWorker>() { subordinate });
        }

        /// <inheritdoc />
        public virtual void RemoveSubordinate(IEnumerable<IWorker> subordinates)
        {
            var existsIds = _subordinates.Select(s => s.Id);
            var existsSubordinates = subordinates.Where(s => !existsIds.Contains(s.Id));
            foreach (var worker in existsSubordinates)
            {
                _subordinates.Remove(worker);
                worker.Boss = null;
            }
        }

        /// <inheritdoc />
        public virtual void ClearSubordinates()
        {
            _subordinates.Clear();
        }
    }
}
