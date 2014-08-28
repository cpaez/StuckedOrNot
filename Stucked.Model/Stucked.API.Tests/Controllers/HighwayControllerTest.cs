using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stucked.API.Controllers;
using Stucked.Model;
using Stucked.Services;
using System.Collections.Generic;

namespace Stucked.API.Tests.Controllers
{
    [TestClass]
    public class HighwayControllerTest
    {
        [TestMethod]
        public void GetHighwayControllerTest()
        {
            // arrange
            var service = new Mock<ITransitStatusService>();
            var controller = new HighwayController(service.Object);

            // act
            var result = controller.Get();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Segment>));
            Assert.IsNotInstanceOfType(result, typeof(IEnumerable<HighwaySign>));
        }
    }
}
