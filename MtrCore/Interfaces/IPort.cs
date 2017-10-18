using System;

namespace MatterCore
{
    public interface IPort
    {
        event Action<IPacket> OnRecv;
        void Transmitt(IPacket packet);
    }
}