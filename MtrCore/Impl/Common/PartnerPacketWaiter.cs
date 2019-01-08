using System.Collections.Generic;
using System.Linq;
using MatterCore;

namespace MtrCore.Impl.Routes
{
    public class PartnerPacketWaiter
    {
        public PartnerPacketWaiter()
        {
        }

        public bool Exists(int label, ISet<IPacket> packets)
        {
            return packets.FirstOrDefault(packet => packet.Label == label) != null;
        }

        public IPacket Pop(int label, ISet<IPacket> packets)
        {
            var packet = packets.First(p => p.Label == label);
            packets.Remove(packet);
            return packet;
        }

        public IPacket Peek(int label, ISet<IPacket> packets)
        {
            return packets.First(p => p.Label == label);
        }
    }
}
