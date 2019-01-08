
using System;
using System.Collections.Generic;
using MatterCore;

namespace MtrCore.Core
{
    public class Core : ICore
    {
        public IInPortBlock InPorts { get; set; }
        public IOutPortBlock OutPorts { get; set; }
        public IRouter Router { get; set; }
        public ISet<IPacket> WaitedPackets { get; set; }
        public IDataTransformer DataTransformer { get; set; }

        public Core(IInPortBlock inPorts, IOutPortBlock outPorts, IRouter router, IDataTransformer dataTransformer)
        {
            InPorts = inPorts;
            OutPorts = outPorts;
            Router = router;
            DataTransformer = dataTransformer;
            WaitedPackets = new HashSet<IPacket>();
        }

        public void TickInput()
        {
        }

        public void TickMiddle()
        {
            while (InPorts.Count() > 0)
            {
                var packet = InPorts.DequeuePacket();

                Router.RoutePacket(packet, this);
            }
        }

        public void TickOutput()
        {
            if (OutPorts is ITickable tickable)
            {
                tickable.Tick();
            }
        }

        public void ConnectPort(ICore other, int selfEdge, int otherEdge)
        {            
            InPorts.Connect(selfEdge, other.OutPorts, otherEdge);
            OutPorts.Connect(selfEdge, other.InPorts, otherEdge);
        }
    }
}
