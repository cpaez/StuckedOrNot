using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stucked.Model;
using System.Collections.Generic;

namespace Stucked.Services.Tests
{
    [TestClass]
    public class TransitStatusServiceTest : BaseStuckedTestFixture
    {
        public ITransitStatusService TransitStatusService;

        [TestInitialize]
        public void Setup()
        {   
            this.TransitStatusService = this.Container.Resolve<ITransitStatusService>();
        }

        [TestMethod]
        public void GetTransitStatusForAllHighwaysTest()
        {
            var result = this.TransitStatusService.GetTransitStatusForAllHighways();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Segment>));
        }

        [TestMethod]
        public void GetTransitStatusForAllHighwaySignsTest()
        {
            var result = this.TransitStatusService.GetTransitStatusForAllHighwaySigns();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<HighwaySignStatus>));
        }
    }
}
