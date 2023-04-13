using System.Collections.Generic;
using Workers.Helpers;
using Workers.Models.Interfaces;
using Workers.Services.Interfaces;

namespace Workers.Services.Calculators
{
    public class SalesCalculator : TopWorkerCalculator
    {
        /// <summary>
        /// Percentage bonus for subordinates
        /// </summary>
        public const double PERCENTAGE_BONUS_FOR_SUBORDINATES = 0.03;

        /// <param name="bonusPerYear">Bonus per year in percent</param>
        /// <param name="maxBonusForYearsOfWork">Maximum bonus for years of work</param>
        /// <param name="calculatorService">Service used to determine which calculators are being used</param>
        public SalesCalculator(double bonusPerYear, double maxBonusForYearsOfWork, ICalculatorService calculatorService)
            : base(bonusPerYear, maxBonusForYearsOfWork, calculatorService, PERCENTAGE_BONUS_FOR_SUBORDINATES)
        { }

        /// <inheritdoc />
        protected override IEnumerable<IWorker> GetSubordinatesForCalculateBonus(ITopWorker worker)
        {
            return worker.GetAllGenerationsOfSubordinates();
        }
    }
}
