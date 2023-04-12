using System;
using Workers.Enums;
using Workers.Models;
using Workers.Services.Calculators;
using Workers.Services.Factories;
using Workers.Services.Interfaces;

namespace Workers.Services
{
    /// <summary>
    /// Service used to determine which calculators are being used
    /// </summary>
    public class CalculatorService : ICalculatorService
    {
        /// <summary>
        /// Employee calculator
        /// </summary>
        private readonly EmployeeCalculator _employeeCalculator;

        /// <summary>
        /// Manager calculator
        /// </summary>
        private readonly ManagerCalculator _managerCalculator;

        /// <summary>
        /// Sales calculator
        /// </summary>
        private readonly SalesCalculator _salesCalculator;

        public CalculatorService()
        {
            _employeeCalculator = (EmployeeCalculator)SalaryCalculatorFactory.Create(WorkerType.Employee, this);

            _managerCalculator = (ManagerCalculator)SalaryCalculatorFactory.Create(WorkerType.Manager, this);

            _salesCalculator = (SalesCalculator)SalaryCalculatorFactory.Create(WorkerType.Sales, this);
        }

        /// <inheritdoc />
        public double CalculateSalary(Employee employee, DateTime payrollDate)
        {
            var salary = _employeeCalculator.CalculateSalary(employee, payrollDate);

            return salary;
        }

        /// <inheritdoc />
        public double CalculateSalary(Manager manager, DateTime payrollDate)
        {
            var salary = _managerCalculator.CalculateSalary(manager, payrollDate);

            return salary;
        }

        /// <inheritdoc />
        public double CalculateSalary(Sales sales, DateTime payrollDate)
        {
            var salary = _salesCalculator.CalculateSalary(sales, payrollDate);

            return salary;
        }
    }
}
