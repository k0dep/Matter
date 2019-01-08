
using MatterCore;

namespace MtrCore
{
    public class Packet : IPacket
    {
        public int Label { get; set; }
        public int Data { get; set; }

        public Packet(int label, int data)
        {
            Label = label;
            Data = data;
        }
    }
}
