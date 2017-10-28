using MatterCore;
using MatterCore.Common;
using MatterCore.Routes;
using MtrCore.Common;
using MtrCore.Routes.Factories;

namespace MtrCore.Routes.ProgramRoutes
{
    [ProgrammRoute(ProgramRoutesId.Transit)]
    public class ProgramTransitRoute : IProgrammRouteMarker
    {
        public void Route(IPacket packet, ICore core)
        {
            var decomposer = new StructureDecompressor(packet.Data);
            decomposer.SkipHalf();

            core.Router.Table.AddRoute(decomposer.DepackHalf(), new TransitRoute(decomposer.DepackHalf(), decomposer.DepackHalf()));
        }
    }
}
