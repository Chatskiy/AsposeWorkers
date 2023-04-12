using System;
using Workers.Models.Interfaces;

namespace Workers.Services.Calculators.Interfaces
{
    public interface ITopWorkerSalaryCalculator : ISalaryCalculator
    {
        /// <summary>
        /// Percentage bonus for subordinates
        /// </summary>
        double PercentageBonusForSubordinates { get; }

        /// <summary>
        /// Calculate bonus for subordnantes
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="payrollDate"></param>
        /// <returns></returns>
        double CalculateBonusForSubordnantes(ITopWorker worker, DateTime payrollDate);
    }
}
