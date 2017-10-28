using System;

namespace MatterCore
{
    public interface IPacket
    {
        int Label { get; set; }
        int Data { get; set; }
    }

    public enum PacketStaticLabels : int
    {
        ProgramPacket = 0xF
    }
}