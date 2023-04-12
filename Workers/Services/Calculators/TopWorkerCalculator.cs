using System;
using System.Collections.Generic;
using System.Linq;
using Workers.Models.Interfaces;
using Workers.Services.Calculators.Interfaces;
using Workers.Services.Interfaces;

namespace Workers.Services.Calculators
{

    public abstract class TopWorkerCalculator : BaseWorkerCalculator, ITopWorkerSalaryCalculator
    {
        /// <summary>
        /// Percentage bonus for subordinates
        /// </summary>
        private readonly double _percentageBonusForSubordinates;

        /// <summary>
        /// Service used to determine which calculators are being used
        /// </summary>
        private readonly ICalculatorService _calculatorService;
        public TopWorkerCalculator(double bonusPerYear, double maxBonusForYearsOfWork, ICalculatorService calculatorService, double percentageBonusForSubordinates)
            : base(bonusPerYear, maxBonusForYearsOfWork)
        {

            _percentageBonusForSubordinates = percentageBonusForSubordinates;
            _calculatorService = calculatorService;
        }

        /// <inheritdoc />
        public double PercentageBonusForSubordinates
        {
            get
            {
                return _percentageBonusForSubordinates;
            }
        }

        /// <inheritdoc />
        public override double CalculateSalary(IWorker worker, DateTime payrollDate)
        {
            var topWorker = (ITopWorker)worker;
            var totalSalary = base.CalculateSalary(topWorker, payrollDate) + CalculateBonusForSubordnantes(topWorker, payrollDate);

            return totalSalary;
        }

        /// <summary>
        /// Calculate bonus for subordnantes
        /// </summary>
        /// <param name="worker">Worker</param>
        /// <param name="payrollDate">Payroll Date</param>
        /// <returns></returns>
        public virtual double CalculateBonusForSubordnantes(ITopWorker worker, DateTime payrollDate)
        {
            var subordinates = GetSubordinatesForCalculateBonus(worker);
            var salaryOfSubordinates = CalculateSalaryOfSubordinates(subordinates, payrollDate);
            var bonusForSubordinates = salaryOfSubordinates * PercentageBonusForSubordinates;

            return bonusForSubordinates;
        }

        /// <summary>
        /// Get subordinates for calculate bonus
        /// </summary>
        /// <param name="worker">head worker</param>
        protected virtual IEnumerable<IWorker> GetSubordinatesForCalculateBonus(ITopWorker worker)
        {
            return worker.Subordinates;
        }

        /// <summary>
        /// Calculate salary of subordinates
        /// </summary>
        /// <param name="subordinates">Subordinates</param>
        /// <param name="payrollDate">Payroll date</param>
        private double CalculateSalaryOfSubordinates(IEnumerable<IWorker> subordinates, DateTime payrollDate)
        {
            double result = 0;
            foreach (var subordinate in subordinates)
            {
                result += subordinate.CalculateSalary(_calculatorService, payrollDate);
            }

            return result;
        }
    }
}
