using System;
using System.Collections.Generic;

namespace MatterCore
{
    public interface IInPortBlock
    {
        IList<IInPort> Ports { get; }

        IPacket DequeuePacket();
        IEnumerable<IPacket> AllInQueue();
        int Count();
        void Connect(int selfPort, IOutPortBlock ports, int targetPort);
    }

    public interface IOutPortBlock
    {
        IList<IOutPort> Ports { get; }

        void Enqueue(int port, IPacket packet);
        void Connect(int selfPort, IInPortBlock ports, int targetPort);
    }
}