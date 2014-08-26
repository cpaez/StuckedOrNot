using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stucked.API.Controllers;
using Stucked.Model;
using Stucked.Services;
using System.Collections.Generic;

namespace Stucked.API.Tests.Controllers
{
    [TestClass]
    public class HighwaySignControllerTest
    {
        [TestMethod]
        public void GetHighwaySignControllerTest()
        {
            // arrange
            var service = new Mock<ITransitStatusService>();
            var controller = new HighwaySignController(service.Object);

            // act
            var result = controller.Get();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<HighwaySignStatus>));
            Assert.IsNotInstanceOfType(result, typeof(IEnumerable<Highway>));
        }
    }
}
