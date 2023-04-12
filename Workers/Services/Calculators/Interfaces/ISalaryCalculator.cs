using System;
using Workers.Models.Interfaces;

namespace Workers.Services.Calculators.Interfaces
{
    public interface ISalaryCalculator
    {
        /// <summary>
        /// Calculate salary for worker
        /// </summary>
        /// <param name="worker">A worker whose salary is calculated</param>
        /// <param name="payrollDate">Date when salary calculated</param>
        /// <returns></returns>
        double CalculateSalary(IWorker worker, DateTime payrollDate);

        /// <summary>
        /// Bonus per year in percent
        /// </summary>
        double BonusPerYear { get; }

        /// <summary>
        /// Maximum bonus for years of work
        /// </summary>
        double MaxBonusForYearsOfWork { get; }
    }
}
