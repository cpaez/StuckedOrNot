using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stucked.Services;
using Stucked.Model;

namespace Stucked.Services.Tests
{
    [TestClass]
    public class TransitStatusServiceTest
    {
        [TestMethod]
        public void GetTransitStatusForAllHighwaysTest()
        {
            var service = new TransitStatusService();

            var result = service.GetTransitStatusForAllHighways();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Highway>));
        }

        [TestMethod]
        public void GetTransitStatusForAllHighwaySignsTest()
        {
            var service = new TransitStatusService();

            var result = service.GetTransitStatusForAllHighwaySigns();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<HighwaySignStatus>));
        }
    }
}
