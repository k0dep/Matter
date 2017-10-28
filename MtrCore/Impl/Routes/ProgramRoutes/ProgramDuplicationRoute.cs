
using MatterCore;
using MatterCore.Routes;
using MtrCore.Common;
using MtrCore.Routes.Factories;

namespace MtrCore.Impl.Routes.ProgramRoutes
{

    [ProgrammRoute(ProgramRoutesId.Duplication)]
    public class ProgramDuplicationRoute : IProgrammRouteMarker
    {
        public IPacketFactory PacketFactory { get; set; }

        public ProgramDuplicationRoute(IPacketFactory packetFactory)
        {
            PacketFactory = packetFactory;
        }

        public void Route(IPacket packet, ICore core)
        {
            var decomposer = new StructureDecompressor(packet.Data);
            decomposer.SkipHalf();

            core.Router.Table.AddRoute(decomposer.DepackHalf(),
                new DuplicationRoute(decomposer.DepackHalf(),
                    decomposer.DepackHalf(),
                    decomposer.DepackHalf(),
                    decomposer.DepackHalf(),
                    PacketFactory));
        }
    }
}
