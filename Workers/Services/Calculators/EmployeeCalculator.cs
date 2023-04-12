namespace Workers.Services.Calculators
{
    public class EmployeeCalculator : BaseWorkerCalculator
    {
        /// <param name="bonusPerYear">Bonus per year in percent</param>
        /// <param name="maxBonusForYearsOfWork">Maximum bonus for years of work</param>
        public EmployeeCalculator(double bonusPerYear, double maxBonusForYearsOfWork)
            : base(bonusPerYear, maxBonusForYearsOfWork)
        { }
    }
}
