using System;
using System.Collections.Generic;

namespace MatterCore
{
    public class InPort : IInPort
    {
        public event Action<IPacket> OnRecv;
        public void Recv(IPacket packet)
        {
            OnRecv?.Invoke(packet);
        }
    }

    public class OutPort : IOutPort
    {
        protected List<IInPort> ListenedPorts;

        public IEnumerable<IInPort> Listeners => ListenedPorts;

        public OutPort()
        {
            ListenedPorts = new List<IInPort>();
        }

        public void Transmitt(IPacket packet)
        {
            foreach (var listenedPort in ListenedPorts)
            {
                listenedPort.Recv(packet);
            }
        }

        public void Connect(IInPort target)
        {
            ListenedPorts.Add(target);
        }
    }
}
