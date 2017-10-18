using System.Collections.Generic;

namespace MatterCore
{
    public interface IRouter
    {
        IRouterTable Table { get; set; }
        void RoutePacket(IPacket packet, ICore core);
    }
}