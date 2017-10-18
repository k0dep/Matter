using System;
using MatterCore;
using MatterCore.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MtrCoreTest.Routes
{
    [TestClass]
    public class TransitRouteTest
    {
        [TestMethod]
        public void TestSetForwardLabel()
        {
            var coreMock = new Mock<ICore>();
            coreMock.SetupProperty(t => t.OutPorts, Mock.Of<IOutPortBlock>());

            var Core = coreMock.Object;

            var packet = new Mock<IPacket>()
                .SetupProperty(t => t.Label)
                .Object;

            var route = new TransitRoute(1, 2);

            route.Route(packet, Core);

            Assert.AreEqual(packet.Label, 2);
        }

        [TestMethod]
        public void TestSetIntagrateWithCore()
        {
            var packet = new Mock<IPacket>()
                .SetupProperty(t => t.Label)
                .Object;

            var calledOutPort = false;
            var outPortBlockMock = new Mock<IOutPortBlock>();
            outPortBlockMock.Setup(t => t.Enqueue(1, packet)).Callback(() => calledOutPort = true);

            var coreMock = new Mock<ICore>();
            coreMock.SetupProperty(t => t.OutPorts, outPortBlockMock.Object);

            var Core = coreMock.Object;

            var route = new TransitRoute(1, 2);

            route.Route(packet, Core);

            Assert.IsTrue(calledOutPort);
        }
    }
}
