using MtrCore.Impl.Routes;

namespace MatterCore.Routes
{
    public class MathFunctionRoute : IRoute
    {
        public int PartnerLabel { get; set; }
        public int OutputPort { get; set; }
        public int OutputLabel { get; set; }
        public int Function { get; set; }

        public PartnerPacketWaiter PartnerWaiter { get; set; }

        public MathFunctionRoute(int partnerLabel, int outputPort, int outputLabel, int function, PartnerPacketWaiter partnerWaiter)
        {
            PartnerLabel = partnerLabel;
            OutputPort = outputPort;
            OutputLabel = outputLabel;
            PartnerWaiter = partnerWaiter;
            Function = function;
        }

        public void Route(IPacket packet, ICore core)
        {
            if (PartnerWaiter.Exists(PartnerLabel, core.WaitedPackets))
            {
                var partnerPacket = PartnerWaiter.Pop(PartnerLabel, core.WaitedPackets);
                packet.Data = core.DataTransformer.TransformBin((DataTransformBinFunction) Function, packet.Data,
                    partnerPacket.Data);
                packet.Label = OutputLabel;
                core.OutPorts.Enqueue(OutputPort, packet);
            }
            else
            {
                core.WaitedPackets.Add(packet);
            }
        }
    }
}