using System;

namespace MatterCore
{
    public interface IPacket
    {
        bool Programm { get; set; }
        int Label { get; set; }
        int Data { get; set; }
    }
}