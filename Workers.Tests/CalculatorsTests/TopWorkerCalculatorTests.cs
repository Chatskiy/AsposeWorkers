using Workers.Enums;
using Workers.Models.Interfaces;
using Workers.Services.Calculators;
using Workers.Services.Calculators.Interfaces;
using Workers.Services.Factories;

namespace Workers.Tests.CalculatorsTests
{
    [TestFixture]
    public abstract class TopWorkerCalculatorTests : BaseWorkerCalculatorTests
    {
        public TopWorkerCalculatorTests(WorkerType workerType)
            : base(workerType)
        { }

        [SetUp]
        public virtual void SetUp()
        {
            ((ITopWorker)Worker).ClearSubordinates();
        }

        [TestCase(1)]
        [TestCase(2)]
        public void CalcSalaryOnlyWithEmployees(int countEmplyees)
        {
            // arrange
            var topWorker = (ITopWorker)Worker;
            var calculator = (ITopWorkerSalaryCalculator)Calculator;
            uint startId = 0;
            PrepareTopWorkeForCalcSalaryOnlyWithEmployees(topWorker, countEmplyees, ref startId);

            var yearNumber = 0;
            var payrollDate = GetSalaryDate(topWorker, yearNumber);

            var salaryExpected = CalculateExpectedSalaryWithYearsBonus(topWorker.Salary, yearNumber, calculator.BonusPerYear);

            var salaryBonusForSubordinatesExpected = CalcSubordinatesEmployeeSalary(topWorker.Subordinates, yearNumber) * calculator.PercentageBonusForSubordinates;

            var salaryTotalExpected = salaryExpected + salaryBonusForSubordinatesExpected;

            // act
            var salaryCalculated = calculator.CalculateSalary(topWorker, payrollDate);

            // assert
            Assert.That(salaryCalculated, Is.EqualTo(salaryTotalExpected).Within(TestingConsts.SALARY_PRECISION));
        }

        [Test]
        public void CalcSalaryForSubordiantesManagerOrSales([Values(1, 2)] int countOfTopWorkers, [Values(1, 2)] int countEmployees, [Values(WorkerType.Manager, WorkerType.Sales)] WorkerType workerType)
        {
            // arrange
            var topWorker = (ITopWorker)Worker;
            var calculator = (ITopWorkerSalaryCalculator)Calculator;
            PrepareWorkerForCalcSalaryWithTopSubordinateAndEmployees(topWorker, workerType, countOfTopWorkers, countEmployees);

            var yearNumber = 0;
            var payrollDate = GetSalaryDate(topWorker, yearNumber);

            var salaryExpected = CalculateExpectedSalaryWithYearsBonus(topWorker.Salary, yearNumber, calculator.BonusPerYear);

            var salaryBonusForSubordinatesExpected = CalcSubordinatesSalary(topWorker.Subordinates, yearNumber) * calculator.PercentageBonusForSubordinates;

            var salaryTotalExpected = salaryExpected + salaryBonusForSubordinatesExpected;

            // act
            var salaryCalculated = calculator.CalculateSalary(topWorker, payrollDate);

            // assert
            Assert.That(salaryCalculated, Is.EqualTo(salaryTotalExpected).Within(TestingConsts.SALARY_PRECISION));
        }

        /// <summary>
        /// Calculate salary manager's subordinates
        /// </summary>
        /// <param name="workers">subordinates</param>
        /// <param name="yearNumber">Number years of experience</param>
        /// <exception cref="ArgumentException">Throw exception if has unsupported worker type</exception>

        protected virtual double CalcSubordinatesSalary(IEnumerable<IWorker> workers, int yearNumber)
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
                            totalSalary += CalcTopWorkerSalaryWithOnlyEmployees((ITopWorker)worker, yearNumber);
                            break;
                        }
                    default:
                        throw new ArgumentException();
                }
            }

            return totalSalary;
        }

        /// <summary>
        /// Calculate salary
        /// </summary>
        /// <param name="topWorker"></param>
        /// <param name="yearNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected double CalcTopWorkerSalaryWithOnlyEmployees(ITopWorker topWorker, int yearNumber)
        {
            double bonusPerYear, percentageBonus;
            switch (topWorker.WorkerType)
            {
                case WorkerType.Manager:
                    {
                        bonusPerYear = SalaryCalculatorFactory.MANAGER_BONUS_PER_YEAR;
                        percentageBonus = ManagerCalculator.PERCENTAGE_BONUS_FOR_SUBORDINATES;
                        break;
                    }
                case WorkerType.Sales:
                    {
                        bonusPerYear = SalaryCalculatorFactory.SALES_BONUS_PER_YEAR;
                        percentageBonus = SalesCalculator.PERCENTAGE_BONUS_FOR_SUBORDINATES;
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }

            var totalEmployeesSalary = CalcSubordinatesEmployeeSalary(topWorker.Subordinates, yearNumber);

            var managerExpectedSalary = CalculateExpectedSalaryWithYearsBonus(topWorker.Salary, yearNumber, bonusPerYear);
            managerExpectedSalary += totalEmployeesSalary * percentageBonus;

            return managerExpectedSalary;
        }

        /// <summary>
        /// Calculate salary for employees
        /// </summary>
        /// <param name="employees">subordinates</param>
        /// <param name="years">Number years of experience</param>
        /// <exception cref="ArgumentException">Throw exception if has unsupported worker type</exception>
        protected double CalcSubordinatesEmployeeSalary(IEnumerable<IWorker> employees, int years)
        {
            var bonusPerYear = SalaryCalculatorFactory.EMPLOYEE_BONUS_PER_YEAR;
            double totalSalary = 0;

            foreach (var employee in employees)
            {
                if (employee.WorkerType == WorkerType.Employee)
                {
                    totalSalary += CalculateExpectedSalaryWithYearsBonus(employee.Salary, years, bonusPerYear);
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            return totalSalary;
        }

        /// <summary>
        /// Create new worker with selected subordinateTopWorkerType and add him to head worker
        /// Fill new worker's subordiante with employees
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="subordinateTopWorkerType"></param>
        /// <param name="countOfTopWorkers"></param>
        /// <param name="countEmployees"></param>
        private void PrepareWorkerForCalcSalaryWithTopSubordinateAndEmployees(ITopWorker worker, WorkerType subordinateTopWorkerType, int countOfTopWorkers, int countEmployees)
        {
            uint id = 0;

            for (int i = 0; i < countOfTopWorkers; i++)
            {
                var subordinateWorker = WorkerFactory.Create(subordinateTopWorkerType);
                subordinateWorker.Id = id;
                worker.AddSubordinate(subordinateWorker);
                id++;

                PrepareTopWorkeForCalcSalaryOnlyWithEmployees((ITopWorker)subordinateWorker, countEmployees, ref id);
            }
        }
    }
}
