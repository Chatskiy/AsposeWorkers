using Microsoft.VisualBasic;
using Workers.Enums;
using Workers.Models;
using Workers.Models.Interfaces;
using Workers.Services;
using Workers.Services.Calculators;
using Workers.Services.Factories;
using Workers.Services.Interfaces;

namespace Workers.Tests.CalculatorsTests
{
    /// <summary>
    /// Test for calculate salary all workers of the company
    /// </summary>
    public class AllWorkersSalaryTest
    {
        private readonly ICalculatorService _calculatorService;

        private readonly IWorkersStorage _workersStorage;

        private readonly IWorkerService _workerService;

        public AllWorkersSalaryTest()
        {
            _calculatorService = new CalculatorService();
            _workersStorage = new WorkersStorage();
            _workerService = new WorkerService(_workersStorage);
        }

        [SetUp]
        public void SetUp()
        {
            _workerService.Clear();
        }

        [Test]
        public void CalculateAllWorkersSalary()
        {
            // arrange
            FillWorkers(_workerService);
            var workers = _workerService.Get();
            var payrollDate = WorkerFactory.EMPLOYEE_DATE.AddSeconds(1);
            var salaryExpected = CalculateWorkersSalary(workers, payrollDate);

            // act
            var salaryCalculated = 0.0;
            foreach (var worker in workers)
            {
                salaryCalculated += worker.CalculateSalary(_calculatorService, payrollDate);
            }
            
            // assert
            Assert.That(salaryCalculated, Is.EqualTo(salaryExpected).Within(TestingConsts.SALARY_PRECISION));
        }

        /// <summary>
        /// Fill service worker's data
        /// </summary>
        /// <param name="service">Worker service</param>
        private void FillWorkers(IWorkerService service)
        {
            // head of company
            var headOfCompany = WorkerFactory.CreateManager();
            service.Add(headOfCompany);
            
            // main advisers
            FillTopWorkerSubordinates(headOfCompany, service);

            foreach (var subordinate in headOfCompany.Subordinates)
            {
                var topWorker = subordinate as ITopWorker;
                if (topWorker != null)
                {
                    FillTopWorkerSubordinatesOfEmployees(topWorker, service);
                }
            }
        }

        /// <summary>
        /// Create main advisers
        /// </summary>
        /// <param name="headOfCompany">Head of company</param>
        /// <param name="service">Worker service</param>
        private void FillTopWorkerSubordinates(ITopWorker headOfCompany, IWorkerService service)
        {
            var expert = WorkerFactory.CreateEmployee();
            service.Add(expert);
            headOfCompany.AddSubordinate(expert);

            var manager = WorkerFactory.CreateManager();
            service.Add(manager);
            headOfCompany.AddSubordinate(manager);

            var sales = WorkerFactory.CreateSales();
            service.Add(sales);
            headOfCompany.AddSubordinate(sales);
        }

        /// <summary>
        /// Create subordinates for main advisers
        /// </summary>
        /// <param name="master">current main adviser</param>
        /// <param name="service"></param>
        private void FillTopWorkerSubordinatesOfEmployees(ITopWorker master, IWorkerService service)
        {
            var firstEmpl = WorkerFactory.CreateEmployee();
            service.Add(firstEmpl);
            master.AddSubordinate(firstEmpl);

            var secondEmpl = WorkerFactory.CreateEmployee();
            service.Add(secondEmpl);
            master.AddSubordinate(secondEmpl);
        }

        /// <summary>
        /// Calculate all workers salary 
        /// </summary>
        /// <param name="workers">Workers</param>
        /// <param name="payrollDate">Payroll date</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Throw exception if has unsupported worker type</exception>
        private double CalculateWorkersSalary(IEnumerable<IWorker> workers, DateTime payrollDate)
        {
            double totalSalary = 0.0;

            foreach (var worker in workers)
            {
                switch (worker.WorkerType)
                {
                    case WorkerType.Employee:
                        {
                            totalSalary += CalculateEmployeeSalary((Employee)worker, payrollDate);
                            break;
                        }
                    case WorkerType.Manager:
                        {
                            totalSalary += CalculateManagerSalary((Manager)worker, payrollDate);
                            break;
                        }
                    case WorkerType.Sales:
                        {
                            totalSalary += CalculateSalesSalary((Sales)worker, payrollDate);
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }
            }

            return totalSalary;
        }

        /// <summary>
        /// Calculate Salary
        /// </summary>
        /// <param name="employee">worker</param>
        /// <param name="payrollDate">Payroll date</param>
        private double CalculateEmployeeSalary(Employee employee, DateTime payrollDate)
        {
            return CalculateBaseWorkerSalary(employee, payrollDate, SalaryCalculatorFactory.EMPLOYEE_BONUS_PER_YEAR, SalaryCalculatorFactory.EMPLOYEE_MAX_BONUS_FOR_YEARS);
        }

        /// <summary>
        /// Calculate Salary
        /// </summary>
        /// <param name="manager">worker</param>
        /// <param name="payrollDate">Payroll date</param>
        private double CalculateManagerSalary(Manager manager, DateTime payrollDate)
        {
            var baseSalary = CalculateBaseWorkerSalary(manager, payrollDate, SalaryCalculatorFactory.MANAGER_BONUS_PER_YEAR, SalaryCalculatorFactory.MANAGER_MAX_BONUS_FOR_YEARS);
            var subordinatesBonus = CalculateWorkersSalary(manager.Subordinates, payrollDate) * ManagerCalculator.PERCENTAGE_BONUS_FOR_SUBORDINATES;
            var totalSalary = baseSalary + subordinatesBonus;

            return totalSalary;
        }

        /// <summary>
        /// Calculate Salary
        /// </summary>
        /// <param name="sales">worker</param>
        /// <param name="payrollDate">Payroll date</param>
        private double CalculateSalesSalary(Sales sales, DateTime payrollDate)
        {
            var baseSalary = CalculateBaseWorkerSalary(sales, payrollDate, SalaryCalculatorFactory.SALES_BONUS_PER_YEAR, SalaryCalculatorFactory.SALES_MAX_BONUS_FOR_YEARS);
            var subordinatesBonus = CalculateWorkersSalary(sales.Subordinates, payrollDate) * SalesCalculator.PERCENTAGE_BONUS_FOR_SUBORDINATES;
            var totalSalary = baseSalary + subordinatesBonus;
            return totalSalary;

        }

        /// <summary>
        /// Calculate worker salary with bonus for years of experience
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="payrollDate"></param>
        /// <param name="bonusPerYear"></param>
        /// <param name="maxYearsBonus"></param>
        /// <returns></returns>
        private double CalculateBaseWorkerSalary(IWorker worker, DateTime payrollDate, double bonusPerYear, double maxYearsBonus)
        {
            var years = CalculateYears(worker.EmploymentDate, payrollDate);
            var multiplier = CalculateMultiplierForBonus(years, bonusPerYear, maxYearsBonus);
            var salary = worker.Salary * multiplier;

            return salary;
        }

        /// <summary>
        /// Calculate difference is years between employment date and payroll date
        /// </summary>
        /// <param name="employmentDate">Employment date </param>
        /// <param name="payrollDate">Payroll date</param>
        /// <returns></returns>
        private int CalculateYears(DateTime employmentDate, DateTime payrollDate)
        {
            var years = DateAndTime.DateDiff(DateInterval.Year, employmentDate, payrollDate);

            return (int)years;
        }

        /// <summary>
        /// Calculate multiplier for bonus for years of experience
        /// </summary>
        /// <param name="years">years of experience</param>
        /// <param name="bonusPerYear">Bonus per year</param>
        /// <param name="maxBonus">Maximum bonus value</param>
        /// <returns></returns>
        private double CalculateMultiplierForBonus(int years, double bonusPerYear, double maxBonus)
        {
            var bonusMultiplierInPercentage = 1 + Math.Min(years * bonusPerYear, maxBonus);
            return bonusMultiplierInPercentage;
        }
    }
}
