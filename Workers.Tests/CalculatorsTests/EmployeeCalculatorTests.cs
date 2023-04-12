using Workers.Enums;

namespace Workers.Tests.CalculatorsTests
{
    [TestFixture]
    public class EmployeeCalculatorTests : BaseWorkerCalculatorTests
    {
        public EmployeeCalculatorTests()
            : base(WorkerType.Employee)
        { }
    }
}
