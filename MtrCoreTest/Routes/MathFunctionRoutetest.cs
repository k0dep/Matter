using System.Collections.Generic;
using System.Configuration;
using MatterCore;
using MatterCore.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MtrCore.Impl.Routes;

namespace MtrCoreTest.Routes
{
    [TestClass]
    public class MathFunctionRoutetest
    {
        [TestMethod]
        public void NormalFlow()
        {
            var inputPacket1 = Mock.Of<IPacket>();
            var inputPacket2 = Mock.Of<IPacket>();

            var outputRecv = false;
            var outPortBlockMock = new Mock<IOutPortBlock>();
            outPortBlockMock
                .Setup(t => t.Enqueue(It.IsAny<int>(), It.IsAny<IPacket>()))
                .Callback(() => outputRecv = true);

            var Core = Mock.Of<ICore>();
            Core.OutPorts = outPortBlockMock.Object;
            Core.WaitedPackets = new HashSet<IPacket>() { inputPacket2 };
            Core.DataTransformer = Mock.Of<IDataTransformer>();

            var route = new MathFunctionRoute(0, 2, 3, 0, new PartnerPacketWaiter());

            route.Route(inputPacket1, Core);

            Assert.IsTrue(outputRecv);
            Assert.AreEqual(0, Core.WaitedPackets.Count);
        }

        [TestMethod]
        public void ShouldCallDataTransform()
        {
            var inputPacket1 = Mock.Of<IPacket>();
            inputPacket1.Data = 100;
            var inputPacket2 = Mock.Of<IPacket>();
            inputPacket2.Data = 200;

            var outputRecv = false;
            var outPortBlockMock = new Mock<IOutPortBlock>();
            outPortBlockMock
                .Setup(t => t.Enqueue(2, It.IsAny<IPacket>()))
                .Callback((int port, IPacket packet) => outputRecv = packet.Data == 300 && packet.Label == 3);

            var Core = Mock.Of<ICore>();
            Core.OutPorts = outPortBlockMock.Object;
            Core.WaitedPackets = new HashSet<IPacket>() { inputPacket2 };

            var dataTransformMock = new Mock<IDataTransformer>();
            dataTransformMock
                .Setup(t => t.TransformBin(It.IsAny<DataTransformBinFunction>(), 100, 200))
                .Returns(300);

            Core.DataTransformer = dataTransformMock.Object;

            var route = new MathFunctionRoute(0, 2, 3, 0, new PartnerPacketWaiter());

            route.Route(inputPacket1, Core);

            Assert.IsTrue(outputRecv);
        }

        [TestMethod]
        public void ShouldAddToWaiting()
        {
            var inputPacket = Mock.Of<IPacket>();

            var Core = Mock.Of<ICore>();
            Core.OutPorts = Mock.Of<IOutPortBlock>();
            Core.WaitedPackets = new HashSet<IPacket>();

            var route = new MathFunctionRoute(1, 2, 3, 0, new PartnerPacketWaiter());

            route.Route(inputPacket, Core);

            Assert.IsTrue(Core.WaitedPackets.Contains(inputPacket));
        }
    }
}
