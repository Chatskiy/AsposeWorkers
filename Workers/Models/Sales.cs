using System;
using Workers.Enums;
using Workers.Services.Interfaces;

namespace Workers.Models
{
    public class Sales : TopWorker
    {
        public Sales(string name, DateTime employmentDate, double salary)
            : base(name, employmentDate, salary, WorkerType.Sales)
        { }

        /// <inheritdoc />
        public override double CalculateSalary(ICalculatorService service, DateTime payrollDate)
        {
            return service.CalculateSalary(this, payrollDate);
        }
    }
}
