using Microsoft.VisualBasic;
using System;
using Workers.Models.Interfaces;
using Workers.Services.Calculators.Interfaces;

namespace Workers.Services.Calculators
{
    public abstract class BaseWorkerCalculator : ISalaryCalculator
    {
        /// <summary>
        /// Bonus per year in percent
        /// </summary>
        private readonly double _bonusPerYear;

        /// <summary>
        /// Maximum bonus for years of work
        /// </summary>
        private readonly double _maxBonusForYearsOfWork;

        /// <param name="bonusPerYear">Bonus per year in percent</param>
        /// <param name="maxBonusForYearsOfWork">Maximum bonus for years of work</param>
        public BaseWorkerCalculator(double bonusPerYear, double maxBonusForYearsOfWork)
        {
            _bonusPerYear = bonusPerYear;
            _maxBonusForYearsOfWork = maxBonusForYearsOfWork;
        }

        /// <inheritdoc />
        public double BonusPerYear
        {
            get
            {
                return _bonusPerYear;
            }
        }

        /// <inheritdoc />
        public double MaxBonusForYearsOfWork
        {
            get
            {
                return _maxBonusForYearsOfWork;
            }
        }

        /// <summary>
        /// Calculate salary for worker
        /// </summary>
        /// <param name="worker">Worker</param>
        /// <param name="payrollDate">Payroll date</param>
        public virtual double CalculateSalary(IWorker worker, DateTime payrollDate)
        {
            var totalSalary = worker.Salary * CalculateMultiplierBonusForYears(worker.EmploymentDate, payrollDate);

            return totalSalary;
        }

        /// <summary>
        /// Calculate multiplier bonus for years
        /// </summary>
        /// <param name="employmentDate">Employment date</param>
        /// <param name="payrollDate">Payroll date</param>
        protected virtual double CalculateMultiplierBonusForYears(DateTime employmentDate, DateTime payrollDate)
        {
            var years = GetCountYears(employmentDate, payrollDate);
            var bonusMultipplierForYears = 1 + Math.Min(years * BonusPerYear, MaxBonusForYearsOfWork);

            return bonusMultipplierForYears;
        }

        /// <summary>
        /// Get difference in years between employment date and payroll date
        /// </summary>
        /// <param name="employmentDate">Employment date</param>
        /// <param name="payrollDate">Payroll date</param>
        /// <exception cref="ArgumentOutOfRangeException">Throw exception if payroll date less than employment</exception>
        private long GetCountYears(DateTime employmentDate, DateTime payrollDate)
        {
            if (payrollDate < employmentDate)
            {
                throw new ArgumentOutOfRangeException("Payroll date could not be early than employment date");
            }

            var years = DateAndTime.DateDiff(DateInterval.Year, employmentDate, payrollDate);

            return years;
        }
    }
}
