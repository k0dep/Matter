
using System.Collections.Generic;
using MtrCore.Core;

namespace MatterCore.Impl
{
    public class Router : IRouter
    {
        public IRouterTable Table { get; set; }

        public Router(IRouterTable table) =>
            Table = table;

        public void RoutePacket(IPacket packet, ICore core)
        {
            Table.GetRoute(packet.Label).Route(packet, core);
        }
    }
}

