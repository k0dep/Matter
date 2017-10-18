using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MatterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MtrCore.Impl.Routes;

namespace MtrCoreTest.Routes
{
    [TestClass]
    public class BufferRiseRouteTest
    {
        [TestMethod]
        public void AddToBuffer()
        {
            var core = new Mock<ICore>()
                .SetupProperty(t => t.WaitedPackets, new HashSet<IPacket>());

            var packet = new Mock<IPacket>();

            var route = new BufferRiseRoute(null, 2, new PartnerPacketWaiter());

            route.Route(packet.Object, core.Object);

            Assert.IsTrue(core.Object.WaitedPackets.Contains(packet.Object));
        }

        [TestMethod]
        public void CallPartnerRoute()
        {
            var partnerPacket = new Mock<IPacket>().SetupProperty(t => t.Label, 2).Object;

            var core = new Mock<ICore>()
                .SetupProperty(t => t.WaitedPackets, new HashSet<IPacket> {partnerPacket});

            var packet = new Mock<IPacket>();

            var partnerCalled = false;
            var partnerRoute = new Mock<IRoute>();
            partnerRoute
                .Setup(t => t.Route(partnerPacket, core.Object))
                .Callback(() => partnerCalled = true);

            var route = new BufferRiseRoute(partnerRoute.Object, 2, new PartnerPacketWaiter());

            route.Route(packet.Object, core.Object);

            Assert.IsTrue(partnerCalled);
        }
    }
}
