using Workers.Enums;
using Workers.Models.Interfaces;

namespace Workers.Tests.CalculatorsTests
{
    [TestFixture]
    public class SalesCalculatorTests : TopWorkerCalculatorTests
    {
        public SalesCalculatorTests()
            : base(WorkerType.Sales)
        { }

        /// <inheritdoc/>
        protected override double CalcSubordinatesSalary(IEnumerable<IWorker> workers, int yearNumber)
        {
            double totalSalary = 0;

            foreach (var worker in workers)
            {
                switch (worker.WorkerType)
                {
                    case WorkerType.Employee:
                        {
                            totalSalary += CalcSubordinatesEmployeeSalary(new List<IWorker>() { worker }, yearNumber);
                            break;
                        }
                    case WorkerType.Manager:
                    case WorkerType.Sales:
                        {
                            var topWorker = (ITopWorker)worker;
                            totalSalary += CalcTopWorkerSalaryWithOnlyEmployees(topWorker, yearNumber);
                            totalSalary += CalcSubordinatesEmployeeSalary(topWorker.Subordinates, yearNumber);
                            break;
                        }
                    default:
                        throw new ArgumentException();
                }
            }

            return totalSalary;
        }
    }
}
