using System;
using Workers.Enums;
using Workers.Services.Interfaces;

namespace Workers.Models
{
    public class Manager : TopWorker
    {
        public Manager(string name, DateTime employmentDate, double salary)
            : base(name, employmentDate, salary, WorkerType.Manager)
        { }

        /// <inheritdoc />
        public override double CalculateSalary(ICalculatorService service, DateTime payrollDate)
        {
            return service.CalculateSalary(this, payrollDate);
        }
    }
}
