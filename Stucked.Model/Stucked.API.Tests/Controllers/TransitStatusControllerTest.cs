using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stucked.API;
using Stucked.API.Controllers;
using Stucked.Model;

namespace Stucked.API.Tests.Controllers
{
    [TestClass]
    public class TransitStatusControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            TransitStatusController controller = new TransitStatusController();

            // Act
            IEnumerable<TransitStatus> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            TransitStatusController controller = new TransitStatusController();

            // Act
            IEnumerable<TransitStatus> result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }
    }
}
