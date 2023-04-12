using System;
using Workers.Enums;
using Workers.Services.Calculators;
using Workers.Services.Calculators.Interfaces;
using Workers.Services.Interfaces;

namespace Workers.Services.Factories
{
    /// <summary>
    /// Factory what is used for create any types of calculators
    /// </summary>
    public static class SalaryCalculatorFactory
    {
        /// <summary>
        /// Bonus per year in percent
        /// </summary>
        public const double EMPLOYEE_BONUS_PER_YEAR = 0.03;

        /// <summary>
        /// Maximum bonus for years of work 
        /// </summary>
        public const double EMPLOYEE_MAX_BONUS_FOR_YEARS = 0.3;

        /// <summary>
        /// Bonus per year in percent
        /// </summary>
        public const double MANAGER_BONUS_PER_YEAR = 0.05;

        /// <summary>
        /// Maximum bonus for years of work 
        /// </summary>
        public const double MANAGER_MAX_BONUS_FOR_YEARS = 0.4;

        /// <summary>
        /// Bonus per year in percent
        /// </summary>
        public const double SALES_BONUS_PER_YEAR = 0.01;

        /// <summary>
        /// Maximum bonus for years of work 
        /// </summary>
        public const double SALES_MAX_BONUS_FOR_YEARS = 0.35;

        /// <summary>
        /// Create calculator for selected type of worker
        /// </summary>
        /// <param name="workerType">Worker type</param>
        /// <param name="_calculatorService">Service used to determine which calculators are being used</param>
        /// <exception cref="NotImplementedException">Throw exception if create calculator for selected workerType has no implement</exception>
        public static ISalaryCalculator Create(WorkerType workerType, ICalculatorService _calculatorService)
        {
            switch (workerType)
            {
                case WorkerType.Employee:
                    {
                        return new EmployeeCalculator(EMPLOYEE_BONUS_PER_YEAR, EMPLOYEE_MAX_BONUS_FOR_YEARS);
                    }
                case WorkerType.Manager:
                    {
                        return new ManagerCalculator(MANAGER_BONUS_PER_YEAR, MANAGER_MAX_BONUS_FOR_YEARS, _calculatorService);
                    }
                case WorkerType.Sales:
                    {
                        return new SalesCalculator(SALES_BONUS_PER_YEAR, SALES_MAX_BONUS_FOR_YEARS, _calculatorService);
                    }
                default:
                    throw new NotImplementedException("Create calculator for selected type of worker do not implemented");
            }
        }
    }
}
