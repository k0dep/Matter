
using System;
using System.Collections.Generic;
using System.Linq;
using MatterCore;
using MtrCore.Interfaces;

namespace MtrCore.Impl.Ports
{
    public class InPortBlock : IInPortBlock
    {
        public IList<IInPort> Ports => inPorts;

        protected List<IInPort> inPorts;
        protected Queue<IPacket> Packets;

        public InPortBlock(int PortCount, IPortFactory portFactory)
        {
            Packets = new Queue<IPacket>();
            inPorts = new List<IInPort>();
            for (int i = 0; i < PortCount; i++)
            {
                var port = portFactory.CreateInputPort();
                port.OnRecv += PortOnOnRecv;
                Ports.Add(port);
            }
        }

        protected virtual void PortOnOnRecv(IPacket packet)
        {
            Packets.Enqueue(packet);
        }

        public IPacket DequeuePacket()
        {
            if(Count() == 0)
                throw new IndexOutOfRangeException();

            return Packets.Dequeue();
        }

        public IEnumerable<IPacket> AllInQueue()
        {
            return Packets;
        }

        public int Count() => Packets.Count;

        public void Connect(int selfPort, IOutPortBlock ports, int targetPort)
        {
            ports.Connect(targetPort, this, selfPort);
        }
    }

    public class OutPortBlock : IOutPortBlock, ITickable
    {
        public IList<IOutPort> Ports => outPorts;

        protected List<IOutPort> outPorts;
        protected List<QueueItem> outQueue;


        public OutPortBlock(int PortCount, IPortFactory portFactory)
        {
            outPorts = new List<IOutPort>();
            for (int i = 0; i < PortCount; i++)
            {
                outPorts.Add(portFactory.CreateOutputPort());
            }
            outQueue = new List<QueueItem>();
        }

        public void Enqueue(int port, IPacket packet)
        {
            outQueue.Add(new QueueItem(port, packet));
        }

        public void Connect(int selfPort, IInPortBlock ports, int targetPort)
        {
            outPorts[selfPort].Connect(ports.Ports[targetPort]);
        }

        protected class QueueItem
        {
            public int port;
            public IPacket packet;

            public QueueItem(int port, IPacket packet)
            {
                this.port = port;
                this.packet = packet;
            }
        }

        public void Tick()
        {
            var for_out = new List<QueueItem>();
            for (int i = 0; i < Ports.Count; i++)
            {
                var item = outQueue.FirstOrDefault(t => t.port == i);
                if(item != null)
                    for_out.Add(item);
            }
            for_out.ForEach(t =>
            {
                outQueue.Remove(t);
                Ports[t.port].Transmitt(t.packet);
            });
        }
    }
}
