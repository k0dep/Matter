using MatterCore;
using MatterCore.Routes;
using MtrCore.Common;
using MtrCore.Impl.Routes;
using MtrCore.Routes.Factories;

namespace MtrCore.Routes.ProgramRoutes
{
    [ProgrammRoute(ProgramRoutesId.MathFuncton)]
    public class ProgramMathFunction : IProgrammRouteMarker
    {
        public PartnerPacketWaiter Waiter { get; set; }

        public ProgramMathFunction(PartnerPacketWaiter waiter)
        {
            Waiter = waiter;
        }

        public void Route(IPacket packet, ICore core)
        {
            var decomposer = new StructureDecompressor(packet.Data);
            decomposer.SkipHalf();

            var mainLabel = decomposer.DepackHalf();
            var bufferLabel = decomposer.DepackHalf();

            var mathRoute = new MathFunctionRoute(bufferLabel, decomposer.DepackHalf(), decomposer.DepackHalf(),
                decomposer.DepackHalf(), Waiter );

            var bufferRoute = new BufferRiseRoute(mathRoute, mainLabel, Waiter);

            core.Router.Table.AddRoute(mainLabel, mathRoute);
            core.Router.Table.AddRoute(bufferLabel, bufferRoute);
        }
    }
}
