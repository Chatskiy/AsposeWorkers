using System;
using Workers.Enums;
using Workers.Models;
using Workers.Models.Interfaces;

namespace Workers.Services.Factories
{

    /// <summary>
    /// Factory what is used for create any types of worker
    /// </summary>
    public static class WorkerFactory
    {
        public const string EMPLOYEE_NAME = "EMPLOYEE_NAME";

        public readonly static DateTime EMPLOYEE_DATE = new DateTime(2023, 1, 5);

        public const double EMPLOYEE_SALARY = 100000;

        public const string MANAGER_NAME = "MANAGER_NAME";

        public readonly static DateTime MANAGER_DATE = new DateTime(2023, 1, 5);

        public const double MANAGER_SALARY = 100000;

        public const string SALES_NAME = "SALES_NAME";

        public readonly static DateTime SALES_DATE = new DateTime(2023, 1, 5);

        public const double SALES_SALARY = 100000;

        /// <summary>
        /// Create new worker with selected workerType and default properties
        /// </summary>
        /// <param name="workerType">Worker type</param>
        /// <exception cref="NotImplementedException">Throw exception if create worker for selected workerType has no implement</exception>
        public static IWorker Create(WorkerType workerType)
        {
            switch (workerType)
            {
                case WorkerType.Employee:
                    return CreateEmployee();
                case WorkerType.Manager:
                    return CreateManager();
                case WorkerType.Sales:
                    return CreateSales();
                default:
                    throw new NotImplementedException("Create worker for selected type of worker do not implemented");
            }
        }

        /// <summary>
        /// Create sales with default properties
        /// </summary>
        public static Employee CreateEmployee()
        {
            return CreateEmployee(EMPLOYEE_NAME, EMPLOYEE_DATE, EMPLOYEE_SALARY);
        }

        /// <summary>
        /// Create employee with new properties
        /// </summary>
        /// <param name="name">Mame</param>
        /// <param name="employmentDate">Employment date</param>
        /// <param name="salary">Salary</param>
        public static Employee CreateEmployee(string name, DateTime employmentDate, double salary)
        {
            return new Employee(name, employmentDate, salary);
        }

        /// <summary>
        /// Create manager with default properties
        /// </summary>
        public static Manager CreateManager()
        {
            return CreateManager(MANAGER_NAME, MANAGER_DATE, MANAGER_SALARY);
        }

        /// <summary>
        /// Create manager with new properties
        /// </summary>
        /// <param name="name">Mame</param>
        /// <param name="employmentDate">Employment date</param>
        /// <param name="salary">Salary</param>
        public static Manager CreateManager(string name, DateTime employmentDate, double salary)
        {
            return new Manager(name, employmentDate, salary);
        }

        /// <summary>
        /// Create sales with default properties
        /// </summary>
        public static Sales CreateSales()
        {
            return CreateSales(SALES_NAME, SALES_DATE, SALES_SALARY);
        }

        /// <summary>
        /// Create sales with new properties
        /// </summary>
        /// <param name="name">Mame</param>
        /// <param name="employmentDate">Employment date</param>
        /// <param name="salary">Salary</param>
        public static Sales CreateSales(string name, DateTime employmentDate, double salary)
        {
            return new Sales(name, employmentDate, salary);
        }
    }
}
