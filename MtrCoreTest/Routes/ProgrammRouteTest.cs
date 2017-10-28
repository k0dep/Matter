using System.Collections.Generic;
using MatterCore;
using MatterCore.Routes;
using Moq;
using MtrCore.Common;
using MtrCore.Impl.Routes;
using MtrCore.Impl.Routes.ProgramRoutes;
using MtrCore.Routes.ProgramRoutes;
using NUnit.Framework;

namespace MtrCoreTest.Routes
{
    [TestFixture]
    public class ProgrammRouteTest
    {
        [Test]
        public void RoutingNormal()
        {
            var inputPacket = Mock.Of<IPacket>();
            inputPacket.Label = (int)PacketStaticLabels.ProgramPacket;

            var core = Mock.Of<ICore>();

            var callNormaly = false;
            var subRouteMock = new Mock<IProgrammRouteMarker>();
            subRouteMock.Setup(t => t.Route(inputPacket, core))
                .Callback(() => callNormaly = true);

            var route = new ProgrammRoute();
            route.RouteProgrammsBases.Add(ProgramRoutesId.Transit, subRouteMock.Object);

            route.Route(inputPacket, core);

            Assert.IsTrue(callNormaly);
        }

        [Test, Combinatorial]
        public void Transit(
                [Range(0, 1)] int OUT_PORT,
                [Range(0, 1)] int OUT_LABEL,
                [Range(0, 1)] int IN_LABEL)
        {
            var inputPacket = Mock.Of<IPacket>();
            inputPacket.Label = (int)PacketStaticLabels.ProgramPacket;

            inputPacket.Data = new StructureCompressor()
                .PackHalf(OUT_LABEL)
                .PackHalf(OUT_PORT)
                .PackHalf(IN_LABEL)
                .PackHalf((int) ProgramRoutesId.Transit)
                .Build();

            var Core = Mock.Of<ICore>();
            Core.OutPorts = Mock.Of<IOutPortBlock>();
            Core.WaitedPackets = new HashSet<IPacket>();
            Core.Router = Mock.Of<IRouter>();

            var callNormaly = false;
            var routerTable = new Mock<IRouterTable>();
            routerTable.Setup(t => t.AddRoute(IN_LABEL, It.IsAny<IRoute>()))
                .Callback((int label, IRoute p_route) =>
                {
                    callNormaly = p_route is TransitRoute transitRoute 
                        && transitRoute.OutputLabel == OUT_LABEL
                        && transitRoute.OutputPort == OUT_PORT;

                });

            Core.Router.Table = routerTable.Object;            

            var route = new ProgrammRoute();
            route.RouteProgrammsBases.Add(ProgramRoutesId.Transit, new ProgramTransitRoute());

            route.Route(inputPacket, Core);

            Assert.IsTrue(callNormaly);
        }

        [Test, Combinatorial]
        public void Duplication(
            [Range(0, 1)] int OUT_ONEPORT,
            [Range(0, 1)] int OUT_ONELABEL,
            [Range(0, 1)] int OUT_TWOPORT,
            [Range(0, 1)] int OUT_TWOLABEL,
            [Range(0, 1)] int IN_LABEL
            )
        {
            var inputPacket = Mock.Of<IPacket>();
            inputPacket.Label = (int)PacketStaticLabels.ProgramPacket;

            inputPacket.Data = new StructureCompressor()
                .PackHalf(OUT_TWOPORT)
                .PackHalf(OUT_TWOLABEL)

                .PackHalf(OUT_ONEPORT)
                .PackHalf(OUT_ONELABEL)

                .PackHalf(IN_LABEL)
                .PackHalf((int) ProgramRoutesId.Duplication)
                .Build();

            var Core = Mock.Of<ICore>();
            Core.OutPorts = Mock.Of<IOutPortBlock>();
            Core.WaitedPackets = new HashSet<IPacket>();
            Core.Router = Mock.Of<IRouter>();

            var callNormaly = false;
            var routerTable = new Mock<IRouterTable>();
            routerTable.Setup(t => t.AddRoute(IN_LABEL, It.IsAny<IRoute>()))
                .Callback((int label, IRoute p_route) =>
                {
                    callNormaly = p_route is DuplicationRoute transitRoute
                                  && transitRoute.BranchALabel == OUT_ONELABEL
                                  && transitRoute.BranchAPort == OUT_ONEPORT
                                  && transitRoute.BranchBLabel == OUT_TWOLABEL
                                  && transitRoute.BranchBPort == OUT_TWOPORT;

                });

            Core.Router.Table = routerTable.Object;

            var route = new ProgrammRoute();
            route.RouteProgrammsBases.Add(ProgramRoutesId.Duplication, new ProgramDuplicationRoute(Mock.Of<IPacketFactory>()));

            route.Route(inputPacket, Core);

            Assert.IsTrue(callNormaly);
        }

        [Test, Combinatorial]
        public void SwitchZero(
            [Range(0, 1)] int OUT_EQPORT,
            [Range(0, 1)] int OUT_EQLABEL,
            [Range(0, 1)] int OUT_NEQPORT,
            [Range(0, 1)] int OUT_NEQLABEL,
            [Range(0, 1)] int IS_FLOAT,
            [Range(0, 1)] int IN_LABEL
        )
        {
            var inputPacket = Mock.Of<IPacket>();
            inputPacket.Label = (int)PacketStaticLabels.ProgramPacket;

            inputPacket.Data = new StructureCompressor()
                .PackHalf(IS_FLOAT)
                .PackHalf(OUT_NEQPORT)
                .PackHalf(OUT_NEQLABEL)

                .PackHalf(OUT_EQPORT)
                .PackHalf(OUT_EQLABEL)

                .PackHalf(IN_LABEL)

                .PackHalf((int)ProgramRoutesId.ZeroSwitch)

                .Build();

            var Core = Mock.Of<ICore>();
            Core.OutPorts = Mock.Of<IOutPortBlock>();
            Core.WaitedPackets = new HashSet<IPacket>();
            Core.Router = Mock.Of<IRouter>();

            var callNormaly = false;
            var routerTable = new Mock<IRouterTable>();
            routerTable.Setup(t => t.AddRoute(IN_LABEL, It.IsAny<IRoute>()))
                .Callback((int label, IRoute p_route) =>
                {
                    callNormaly = p_route is SwitchZeroRoute rt
                                  && rt.ZeroEqualLabel == OUT_EQLABEL
                                  && rt.ZeroEqualPort == OUT_EQPORT
                                  && rt.NotZeroEqualLabel == OUT_NEQLABEL
                                  && rt.NotZeroEqualPort == OUT_NEQPORT
                                  && rt.DataIsFloat == (IS_FLOAT == 1);
                });

            Core.Router.Table = routerTable.Object;

            var route = new ProgrammRoute();
            route.RouteProgrammsBases.Add(ProgramRoutesId.ZeroSwitch, new ProgramZeroSwitchRoute());

            route.Route(inputPacket, Core);

            Assert.IsTrue(callNormaly);
        }

        [Test, Combinatorial]
        public void MathFunction(
            [Range(0, 1)] int BUFER_ROUTE,
            [Range(2, 3)] int OUT_LABEL,
            [Range(3, 4)] int OUT_PORT,
            [Range(5, 6)] int FUNCTION,
            [Range(7, 8)] int IN_LABEL
        )
        {
            var inputPacket = Mock.Of<IPacket>();
            inputPacket.Label = (int)PacketStaticLabels.ProgramPacket;

            inputPacket.Data = new StructureCompressor()

                .PackHalf(FUNCTION)
                .PackHalf(OUT_LABEL)
                .PackHalf(OUT_PORT)
                .PackHalf(BUFER_ROUTE)

                .PackHalf(IN_LABEL)
                .PackHalf((int)ProgramRoutesId.MathFuncton)

                .Build();

            var Core = Mock.Of<ICore>();
            Core.OutPorts = Mock.Of<IOutPortBlock>();
            Core.WaitedPackets = new HashSet<IPacket>();
            Core.Router = Mock.Of<IRouter>();

            var createdGenreal = false;
            IRoute generalRoute = null;
            var createdBuffer = false;

            var waiter = new PartnerPacketWaiter();

            var routerTable = new Mock<IRouterTable>();
            routerTable.Setup(t => t.AddRoute(IN_LABEL, It.IsAny<IRoute>()))
                .Callback((int label, IRoute p_route) =>
                {
                    createdGenreal = p_route is MathFunctionRoute rt
                                     && rt.PartnerLabel == BUFER_ROUTE
                                     && rt.OutputPort == OUT_PORT
                                     && rt.OutputLabel == OUT_LABEL
                                     && rt.Function == FUNCTION
                                     && rt.PartnerWaiter == waiter;
                    generalRoute = p_route;
                });

            routerTable.Setup(t => t.AddRoute(BUFER_ROUTE, It.IsAny<IRoute>()))
                .Callback((int label, IRoute p_route) =>
                {
                    createdBuffer = p_route is BufferRiseRoute rt
                                    && rt.PartnerWaiter == waiter
                                    && rt.PartnerLabel == IN_LABEL
                                    && rt.PartnerRoute == generalRoute;
                });

            Core.Router.Table = routerTable.Object;

            var route = new ProgrammRoute();
            route.RouteProgrammsBases.Add(ProgramRoutesId.MathFuncton, new ProgramMathFunction(waiter));

            route.Route(inputPacket, Core);

            Assert.IsTrue(createdBuffer && createdGenreal);
        }

    }
}
