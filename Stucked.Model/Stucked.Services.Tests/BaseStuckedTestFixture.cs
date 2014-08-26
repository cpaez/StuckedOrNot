using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stucked.Services.Tests
{
    [TestClass]
    public class BaseStuckedTestFixture
    {
        public UnityContainer Container;

        [TestInitialize]
        public void SetUp()
        {
            this.Container = new UnityContainer();
            this.Container.RegisterType<ITransitStatusService, TransitStatusService>(new HierarchicalLifetimeManager());
        }
    }
}
