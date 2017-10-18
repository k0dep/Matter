namespace MatterCore.Routes
{
    public class SwitchZeroRoute : IRoute
    {
        public int ZeroEqualLabel { get; set; }
        public int ZeroEqualPort { get; set; }

        public int NotZeroEqualLabel { get; set; }
        public int NotZeroEqualPort { get; set; }

        public SwitchZeroRoute(int zeroEqualLabel, int zeroEqualPort, int notZeroEqualLabel, int notZeroEqualPort)
        {
            ZeroEqualLabel = zeroEqualLabel;
            ZeroEqualPort = zeroEqualPort;
            NotZeroEqualLabel = notZeroEqualLabel;
            NotZeroEqualPort = notZeroEqualPort;
        }

        public void Route(IPacket packet, ICore core)
        {
            var port = NotZeroEqualPort;
            if (packet.Data == 0)
            {
                port = ZeroEqualPort;
                packet.Label = ZeroEqualLabel;
            }
            else
                packet.Label = NotZeroEqualLabel;


            core.OutPorts.Enqueue(port, packet);
        }
    }
}