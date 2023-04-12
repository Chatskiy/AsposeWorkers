using Workers.Enums;

namespace Workers.Tests.CalculatorsTests
{
    [TestFixture]
    public class ManagerCalculatorTests : TopWorkerCalculatorTests
    {
        public ManagerCalculatorTests()
            : base(WorkerType.Manager)
        { }
    }
}
