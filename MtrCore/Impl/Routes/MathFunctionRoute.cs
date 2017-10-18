using MtrCore.Impl.Routes;

namespace MatterCore.Routes
{
    public class MathFunctionRoute : IRoute
    {
        public int PartnerLabel { get; set; }
        public int OutputPort { get; set; }
        public int OutputLabel { get; set; }

        public PartnerPacketWaiter PartnerWaiter { get; set; }

        public MathFunctionRoute(int partnerLabel, int outputPort, int outputLabel, PartnerPacketWaiter partnerWaiter)
        {
            PartnerLabel = partnerLabel;
            OutputPort = outputPort;
            OutputLabel = outputLabel;
            PartnerWaiter = partnerWaiter;
        }

        public void Route(IPacket packet, ICore core)
        {
        }
    }
}