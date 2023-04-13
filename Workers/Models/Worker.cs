using System;
using Workers.Enums;
using Workers.Models.Interfaces;
using Workers.Services.Interfaces;

namespace Workers.Models
{
    public abstract class Worker : IWorker
    {
        private uint _id;

        private readonly string _name;

        private readonly DateTime _employmentDate;

        private readonly double _salary;

        private readonly WorkerType _workerType;

        private IWorker _boss;

        public Worker(string name, DateTime employmentDate, double salary, WorkerType workerType)
        {
            ValidateParams(name, salary);

            _name = name;
            _employmentDate = employmentDate;
            _salary = salary;
            _workerType = workerType;
        }

        public uint Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public DateTime EmploymentDate
        {
            get
            {
                return _employmentDate;
            }
        }

        public double Salary
        {
            get
            {
                return _salary;
            }
        }

        public WorkerType WorkerType
        {
            get
            {
                return _workerType;
            }
        }

        public IWorker Boss
        {
            get
            {
                return _boss;
            }

            set
            {
                _boss = value;
            }
        }

        public abstract double CalculateSalary(ICalculatorService service, DateTime payrollDate);

        /// <summary>
        /// Validate params uset to create new instance
        /// </summary>
        /// <param name="name">Name </param>
        /// <param name="salary">Salary</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ValidateParams(string name, double salary)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name), $"Parameter {nameof(name)} should not be null or empty.");
            }

            if (salary <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(salary), $"Parameter {nameof(salary)} should not be less than or equal to zero.");
            }

        }
    }
}
