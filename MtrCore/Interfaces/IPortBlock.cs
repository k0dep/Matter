using System;
using System.Collections.Generic;

namespace MatterCore
{
    public interface IPortBlock
    {
        IList<IPort> Ports { get; set; }
    }

    public interface IInPortBlock : IPortBlock
    {
        IPacket DequeuePacket();
        IEnumerable<IPacket> AllInQueue();
    }

    public interface IOutPortBlock : IPortBlock
    {
        void Enqueue(int port, IPacket packet);
    }
}