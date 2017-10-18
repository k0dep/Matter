namespace MatterCore.Routes
{
    public class TransitRoute : IRoute
    {
        public int OutputPort { get; set; }
        public int OutputLabel { get; set; }

        public TransitRoute(int outputPort, int outputLabel)
        {
            OutputPort = outputPort;
            OutputLabel = outputLabel;
        }

        public void Route(IPacket packet, ICore core)
        {
            packet.Label = OutputLabel;
            core.OutPorts.Enqueue(OutputPort, packet);
        }
    }
}
