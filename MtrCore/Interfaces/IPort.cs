using System;
using System.Collections.Generic;

namespace MatterCore
{
    public interface IInPort
    {
        event Action<IPacket> OnRecv;
        void Recv(IPacket packet);
    }

    public interface IOutPort
    {
        IEnumerable<IInPort> Listeners { get; }
        void Transmitt(IPacket packet);
        void Connect(IInPort target);
    }
}