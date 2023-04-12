using System;
using Workers.Models;

namespace Workers.Services.Interfaces
{
    /// <summary>
    /// Service used to determine which calculators are being used
    /// </summary>
    public interface ICalculatorService
    {
        /// <summary>
        /// Calculate salary for Employee
        /// </summary>
        /// <param name="employee">Worker</param>
        /// <param name="payrollDate">Salary date</param>
        /// <returns>Salary value</returns>
        double CalculateSalary(Employee employee, DateTime payrollDate);

        /// <summary>
        /// Calculate salary for manager
        /// </summary>
        /// <param name="employee">Worker</param>
        /// <param name="payrollDate">Salary date</param>
        /// <returns>Salary value</returns>
        double CalculateSalary(Manager manager, DateTime payrollDate);

        /// <summary>
        /// Calculate salary for sales
        /// </summary>
        /// <param name="employee">Worker</param>
        /// <param name="payrollDate">Salary date</param>
        /// <returns>Salary value</returns>
        double CalculateSalary(Sales sales, DateTime payrollDate);
    }
}
