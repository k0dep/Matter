using MatterCore;
using MatterCore.Routes;
using MtrCore.Common;
using MtrCore.Routes.Factories;

namespace MtrCore.Routes.ProgramRoutes
{
    [ProgrammRoute(ProgramRoutesId.Transit)]
    public class ProgramZeroSwitchRoute : IProgrammRouteMarker
    {
        public void Route(IPacket packet, ICore core)
        {
            var decomposer = new StructureDecompressor(packet.Data);
            decomposer.SkipHalf();

            core.Router.Table.AddRoute(decomposer.DepackHalf(),
                new SwitchZeroRoute(
                    decomposer.DepackHalf(),
                    decomposer.DepackHalf(),
                    decomposer.DepackHalf(),
                    decomposer.DepackHalf(),
                    decomposer.DepackHalf() == 1));
        }
    }
}
