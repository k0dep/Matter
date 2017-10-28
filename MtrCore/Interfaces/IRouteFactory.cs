
using MatterCore;

namespace MtrCore.Routes
{
    public interface IRouteFactory
    {
        IRoute Create(IPacketFactory packetFactory);
    }
}
