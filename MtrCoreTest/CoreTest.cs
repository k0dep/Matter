using MatterCore;
using MatterCore.Impl;
using MatterCore.Routes;
using Moq;
using MtrCore;
using MtrCore.Core;
using MtrCore.Impl.Ports;
using MtrCore.Impl.Routes;
using MtrCore.Interfaces;
using NUnit.Framework;

namespace MtrCoreTest
{
    [TestFixture]
    public class CoreTest
    {
        [Test]
        public void Ticking()
        {
            var packet = Mock.Of<IPacket>();

            var router = new Mock<IRouter>();
            var inPorts = new Mock<IInPortBlock>();
            var outPorts = new Mock<IOutPortBlock>();
            var dataTransform = Mock.Of<IDataTransformer>();

            inPorts.SetupSequence(t => t.Count())
                .Returns(1)
                .Returns(0);
            inPorts.Setup(t => t.DequeuePacket()).Returns(packet);

            var tickableOut = outPorts.As<ITickable>();

            var core = new Core(inPorts.Object, outPorts.Object, router.Object, dataTransform);

            core.TickInput();
            core.TickMiddle();
            core.TickOutput();

            router.Verify(t => t.RoutePacket(packet, core), Times.Once());
            inPorts.Verify(t => t.Count(), Times.Exactly(2));
            tickableOut.Verify(t => t.Tick(), Times.Once());
        }

        [Test]
        public void ProcessorAdding()
        {
            var c1 = CreateSimpleCore();
            var c2 = CreateSimpleCore();
            var c3 = CreateSimpleCore();
            var c4 = CreateSimpleCore();

            connectNetwork(c1, c2, c3, c4);

            patchingAdding(c1, c2, c3, c4);

            var testInPort = new InPortBlock(1, createSimplePortFactory());
            c3.OutPorts.Connect(3, testInPort, 0);

            setData(c1, c2);

            coresTick(4, c1, c2, c3, c4);

            var packet = testInPort.DequeuePacket();

            Assert.NotNull(packet);
            Assert.AreEqual(0, packet.Label);
            Assert.AreEqual(15, packet.Data);
        }

        private static void connectNetwork(Core c1, Core c2, Core c3, Core c4)
        {
            c1.ConnectPort(c2, 2, 0);
            c1.ConnectPort(c3, 3, 1);

            c4.ConnectPort(c2, 1, 3);
            c4.ConnectPort(c3, 0, 2);
        }

        private void coresTick(int ticks, params ICore[] cores)
        {
            for (var i = 0; i < ticks; i++)
            {
                foreach (var core in cores)
                    core.TickInput();

                foreach (var core in cores)
                    core.TickMiddle();

                foreach (var core in cores)
                    core.TickOutput();               
            }
        }

        private static void setData(ICore c1, ICore c2)
        {
            c1.InPorts.Ports[0].Recv(new Packet(1, 1));
            c1.InPorts.Ports[1].Recv(new Packet(0, 2));

            c2.InPorts.Ports[1].Recv(new Packet(1, 3));
            c2.InPorts.Ports[2].Recv(new Packet(2, 4));
        }

        private static void patchingAdding(Core c1, Core c2, Core c3, Core c4)
        {
            var addRoute =
                            new MathFunctionRoute(0, 3, 1, (int)DataTransformBinFunction.Add, new PartnerPacketWaiter());
            c1.Router.Table.AddRoute(1, addRoute);
            c1.Router.Table.AddRoute(0, new BufferRiseRoute(addRoute, 1));

            var mulRoute = new MathFunctionRoute(2, 3, 1, (int)DataTransformBinFunction.Mul, new PartnerPacketWaiter());
            c2.Router.Table.AddRoute(1, mulRoute);
            c2.Router.Table.AddRoute(2, new BufferRiseRoute(mulRoute, 1));

            c4.Router.Table.AddRoute(1, new TransitRoute(0, 2));

            var addDeltaRoute =
                new MathFunctionRoute(2, 3, 0, (int)DataTransformBinFunction.Add, new PartnerPacketWaiter());
            c3.Router.Table.AddRoute(1, addDeltaRoute);
            c3.Router.Table.AddRoute(2, new BufferRiseRoute(addDeltaRoute, 1));
        }

        Core CreateSimpleCore()
        {
            var portFactory = createSimplePortFactory();

            var router = new Router(new RouterTable());
            var inPBlock = new InPortBlock(4, portFactory);
            var outPBlock = new OutPortBlock(4, portFactory);
            var dataTransformer = new DataTransformer();

            var core = new Core(inPBlock, outPBlock, router, dataTransformer);
            return core;
        }

        IPortFactory createSimplePortFactory()
        {
            var portFactory = new Mock<IPortFactory>();
            portFactory.Setup(t => t.CreateInputPort()).Returns(() => new InPort());
            portFactory.Setup(t => t.CreateOutputPort()).Returns(() => new OutPort());
            return portFactory.Object;
        }
    }
}
