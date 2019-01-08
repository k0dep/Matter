using MatterCore;

namespace MtrCore.Impl.Routes
{
    public class BufferRiseRoute : IRoute
    {
        public int PartnerLabel { get; set; }
        public IRoute PartnerRoute { get; set; }
        public PartnerPacketWaiter PartnerWaiter { get; set; }

        public BufferRiseRoute(IRoute partnerRoute, int partnerLabel)
            : this(partnerRoute, partnerLabel, new PartnerPacketWaiter())
        {
            
        }

        public BufferRiseRoute(IRoute partnerRoute, int partnerLabel, PartnerPacketWaiter partnerWaiter)
        {
            PartnerLabel = partnerLabel;
            PartnerRoute = partnerRoute;
            PartnerWaiter = partnerWaiter;
        }

        public void Route(IPacket packet, ICore core)
        {
            if (PartnerWaiter.Exists(PartnerLabel, core.WaitedPackets))
            {
                var partnerPakcet = PartnerWaiter.Pop(PartnerLabel, core.WaitedPackets);
                core.WaitedPackets.Add(packet);
                PartnerRoute.Route(partnerPakcet, core);
            }
            else
                core.WaitedPackets.Add(packet);
        }
    }
}
