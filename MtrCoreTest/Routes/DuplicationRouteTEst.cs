using System;
using System.Collections.Generic;
using MatterCore;
using MatterCore.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MtrCoreTest.Routes
{
    [TestClass]
    public class DuplicationRouteTest
    {
        [TestMethod]
        public void TestPacketSendTuppleToPorts()
        {
            var inputPacket = new Mock<IPacket>()
                .SetupProperty(t => t.Label)
                .Object;

            var outputPacketList = new List<IPacket>();

            var outPortBlockMock = new Mock<IOutPortBlock>();
            outPortBlockMock
                .Setup(t => t.Enqueue(It.IsAny<int>(), It.IsAny<IPacket>()))
                .Callback((int port, IPacket packet) => outputPacketList.Add(packet));

            var Core = Mock.Of<ICore>();
            Core.OutPorts = outPortBlockMock.Object;

            var packetFactoryMock = new Mock<IPacketFactory>();
            packetFactoryMock.Setup(t => t.Create()).Returns(Mock.Of<IPacket>());

            var route = new DuplicationRoute(10, 11, 20, 21, packetFactoryMock.Object);

            route.Route(inputPacket, Core);

            Assert.AreEqual(2, outputPacketList.Count);
        }

        [TestMethod]
        public void TestPacketSendCorrectLabelData()
        {
            var inputPacket = new Mock<IPacket>()
                .SetupProperty(t => t.Label)
                .Object;

            IPacket packetFromPortA = null;
            IPacket packetFromPortB = null;

            var outPortBlockMock = new Mock<IOutPortBlock>();
            outPortBlockMock
                .Setup(t => t.Enqueue(11, It.IsAny<IPacket>()))
                .Callback((int port, IPacket packet) => packetFromPortA = packet);

            outPortBlockMock
                .Setup(t => t.Enqueue(21, It.IsAny<IPacket>()))
                .Callback((int port, IPacket packet) => packetFromPortB = packet);

            var Core = Mock.Of<ICore>();
            Core.OutPorts = outPortBlockMock.Object;

            var packetFactoryMock = new Mock<IPacketFactory>();
            packetFactoryMock.Setup(t => t.Create()).Returns(Mock.Of<IPacket>());

            var route = new DuplicationRoute(10, 11, 20, 21, packetFactoryMock.Object);

            route.Route(inputPacket, Core);

            Assert.AreEqual(10, packetFromPortA.Label);
            Assert.AreEqual(20, packetFromPortB.Label);
        }
    }
}
