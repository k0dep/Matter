using System.Collections.Generic;
using MtrCore.Common;

namespace MatterCore.Routes
{
    public enum ProgramRoutesId : int
    {
        Transit = 0x0,
        Duplication,
        ZeroSwitch,
        MathFuncton
    }

    public class ProgrammRoute : IRoute
    {
        public Dictionary<ProgramRoutesId, IProgrammRouteMarker> RouteProgrammsBases { get; set; }

        public ProgrammRoute()
        {
            RouteProgrammsBases = new Dictionary<ProgramRoutesId, IProgrammRouteMarker>();
        }

        public void Route(IPacket packet, ICore core)
        {
            var routeId = new StructureDecompressor(packet.Data).DepackHalf();

            RouteProgrammsBases[(ProgramRoutesId)routeId].Route(packet, core);
        }
    }

    public interface IProgrammRouteMarker : IRoute
    {        
    }
}