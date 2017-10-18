namespace MatterCore.Routes
{
    public class DuplicationRoute : IRoute
    {
        public int BranchALabel { get; set; }
        public int BranchAPort { get; set; }

        public int BranchBLabel { get; set; }
        public int BranchBPort { get; set; }

        public IPacketFactory PacketFactory { get; set; }

        public DuplicationRoute(int branchALabel, int branchAPort, int branchBLabel, int branchBPort, IPacketFactory packetFactory)
        {
            BranchALabel = branchALabel;
            BranchAPort = branchAPort;
            BranchBLabel = branchBLabel;
            BranchBPort = branchBPort;
            PacketFactory = packetFactory;
        }

        public void Route(IPacket packet, ICore core)
        {
            packet.Label = BranchALabel;

            var branchBPacket = PacketFactory.Create();
            branchBPacket.Label = BranchBLabel;
            branchBPacket.Data = packet.Data;
            branchBPacket.Programm = packet.Programm;

            core.OutPorts.Enqueue(BranchAPort, packet);
            core.OutPorts.Enqueue(BranchBPort, branchBPacket);
        }
    }
}