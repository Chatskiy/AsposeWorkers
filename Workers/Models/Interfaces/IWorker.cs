using System;
using Workers.Enums;
using Workers.Services.Interfaces;

namespace Workers.Models.Interfaces
{

    public interface IWorker
    {
        /// <summary>
        /// Worker ID
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Employee name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Employment date
        /// </summary>
        DateTime EmploymentDate { get; }

        /// <summary>
        /// Base salary per year
        /// </summary>
        double Salary { get; }

        /// <summary>
        /// Boss
        /// </summary>
        IWorker Boss { get; set; }

        /// <summary>
        /// Worker type
        /// </summary>
        WorkerType WorkerType { get; }

        /// <summary>
        /// Calculate salary for selected date
        /// </summary>
        /// <param name="service">Service used to determine which calculators are being used</param>
        /// <param name="payrollDate">Payroll date</param>
        double CalculateSalary(ICalculatorService service, DateTime payrollDate);
    }
}
