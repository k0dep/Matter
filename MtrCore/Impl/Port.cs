using System;
using MatterCore;

namespace MatterCore.Impl
{
    public class Port : IPort
    {
        public event Action<IPacket> OnRecv;

        public void Transmitt(IPacket packet) =>  OnRecv?.Invoke(packet);
    }
}
