using Workers.Enums;
using Workers.Models.Interfaces;
using Workers.Services;
using Workers.Services.Calculators.Interfaces;
using Workers.Services.Factories;

namespace Workers.Tests.CalculatorsTests
{

    [TestFixture]
    public abstract class BaseWorkerCalculatorTests
    {
        /// <summary>
        /// Salary calculator
        /// </summary>
        private readonly ISalaryCalculator _calculator;

        /// <summary>
        /// Worker
        /// </summary>
        private readonly IWorker _worker;

        /// <summary>
        /// Maximum count of experience with salary bonus
        /// </summary>
        private readonly int _maxCountYears;

        /// <param name="workerType">Define created classes of worker and calculator</param>
        public BaseWorkerCalculatorTests(WorkerType workerType)
        {
            _calculator = SalaryCalculatorFactory.Create(workerType, new CalculatorService());
            _worker = WorkerFactory.Create(workerType);
            _maxCountYears = (int)(_calculator.MaxBonusForYearsOfWork / _calculator.BonusPerYear);
        }

        /// <summary>
        /// Salary calculator
        /// </summary>
        protected ISalaryCalculator Calculator
        {
            get
            {
                return _calculator;
            }
        }

        /// <summary>
        /// Worker
        /// </summary>
        protected IWorker Worker
        {
            get
            {
                return _worker;
            }
        }

        /// <summary>
        /// Maximum count of experience with salary bonus
        /// </summary>
        protected int MaxCountYears
        {
            get
            {
                return _maxCountYears;
            }
        }

        [Test]
        public void SalaryDateLessThanEmploymentDateShoudThrowArgumentOutOfRangeException()
        {
            // arrange
            var worker = Worker;
            var date = worker.EmploymentDate;
            var payrollDate = date.AddSeconds(-1);
            TestDelegate act = () =>
            {
                Calculator.CalculateSalary(worker, payrollDate);
            };

            // act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(act);

            // assert
            Assert.NotNull(ex);
        }

        [Test]
        public void CheckWithoutSubordinatesCalculateSalaryIncludeMaximumYearsBonus()
        {
            // arrange
            var worker = Worker;
            var calculator = Calculator;

            for (int yearNumber = 0; yearNumber <= MaxCountYears; yearNumber++)
            {
                // arrange
                var salaryExpected = CalculateExpectedSalaryWithYearsBonus(worker.Salary, yearNumber, calculator.BonusPerYear);

                var payrollDate = GetSalaryDate(worker, yearNumber);

                // act
                var salaryCalculated = calculator.CalculateSalary(worker, payrollDate);

                // assert
                Assert.That(salaryCalculated, Is.EqualTo(salaryExpected).Within(TestingConsts.SALARY_PRECISION));
            }
        }

        [Test]
        public void CheckWithoutSubordinatesSalaryShoudNotBeMoreThenSalaryMultipliedtoMaxYearsBonus()
        {
            // arrange
            var worker = Worker;
            var calculator = Calculator;

            var salaryExpected = CalculateExpectedSalaryWithYearsBonus(worker.Salary, MaxCountYears, calculator.BonusPerYear);

            var yearNumber = MaxCountYears + 1;
            var payrollDate = GetSalaryDate(worker, yearNumber);

            // act
            var salaryCalculated = calculator.CalculateSalary(worker, payrollDate);

            // assert
            Assert.That(salaryCalculated, Is.EqualTo(salaryExpected).Within(TestingConsts.SALARY_PRECISION));
        }

        /// <summary>
        /// Calculate date added years to worker employment date
        /// </summary>
        /// <param name="worker">Worker</param>
        /// <param name="countAddedYears">Count added years</param>
        /// <returns></returns>
        protected DateTime GetSalaryDate(IWorker worker, int countAddedYears)
        {
            var date = worker.EmploymentDate;
            var payrollDate = date.AddYears(countAddedYears);

            return payrollDate;
        }

        /// <summary>
        /// Calculate salary with years experience bonus
        /// </summary>
        /// <param name="baseSalary">Base salary</param>
        /// <param name="years">Years experience </param>
        /// <param name="bonusPerYear">Bonus per year</param>
        /// <returns></returns>
        protected double CalculateExpectedSalaryWithYearsBonus(double baseSalary, int years, double bonusPerYear)
        {
            var bonusMultiplierInPercentage = 1 + years * bonusPerYear;
            var salaryExpected = baseSalary * bonusMultiplierInPercentage;

            return salaryExpected;
        }

        /// <summary>
        /// Fill worker's subordinates with employees
        /// </summary>
        /// <param name="worker"Worker</param>
        /// <param name="countEmplyees">Count of employees</param>
        /// <param name="startId">Start identifier</param>
        protected void PrepareTopWorkeForCalcSalaryOnlyWithEmployees(ITopWorker worker, int countEmplyees, ref uint startId)
        {
            for (uint index = 0; index < countEmplyees; index++)
            {
                var employee = WorkerFactory.CreateEmployee();
                employee.Id = startId++;
                worker.AddSubordinate(employee);
            }
        }
    }
}