using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MatterCore;
using MatterCore.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MtrCoreTest.Routes
{
    [TestClass]
    public class SwitchZeroRouteTest
    {
        const int TRUE_PORT = 1;
        const int FALSE_PORT = 2;
        const int TRUE_LABEL = 3;
        const int FALSE_LABEl = 4;

        [TestMethod]
        public void TestTrueFlow()
        {
            Assert.IsTrue(IsTrueFlow(0));
        }

        [TestMethod]
        public void TestFalseFlow()
        {
            Assert.IsTrue(!IsTrueFlow(1));
        }

        [TestMethod]
        public void TestLabelConsistent()
        {
            var isTrueLabel = false;
            PostprocessPacket(0, (i, packet) =>
            {
                if (packet.Label == TRUE_LABEL)
                    isTrueLabel = true;
            });

            Assert.IsTrue(isTrueLabel);
        }

        [TestMethod]
        public void TestLabelFalseFlowConsistent()
        {
            var isTrueLabel = false;
            PostprocessPacket(1, (i, packet) =>
            {
                if (packet.Label == FALSE_LABEl)
                    isTrueLabel = true;
            });

            Assert.IsTrue(isTrueLabel);
        }

        public bool IsTrueFlow(int data)
        {
            var resultPortId = -1;

            PostprocessPacket(data, (port, packet) => resultPortId = port);

            return resultPortId == TRUE_PORT;
        }

        public void PostprocessPacket(int data, Action<int, IPacket> onOutQueueAction)
        {
            var inputPacket = Mock.Of<IPacket>();
            inputPacket.Data = data;

            var outPortBlockMock = new Mock<IOutPortBlock>();
            outPortBlockMock
                .Setup(t => t.Enqueue(It.IsAny<int>(), It.IsAny<IPacket>()))
                .Callback(onOutQueueAction);

            var Core = Mock.Of<ICore>();
            Core.OutPorts = outPortBlockMock.Object;

            var route = new SwitchZeroRoute(TRUE_LABEL, TRUE_PORT, FALSE_LABEl, FALSE_PORT);

            route.Route(inputPacket, Core);;
        }
    }
}
