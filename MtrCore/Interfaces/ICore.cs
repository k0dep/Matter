using System.Collections.Generic;

namespace MatterCore
{
    public interface ICore
    {
        IInPortBlock InPorts { get; set; }
        IOutPortBlock OutPorts { get; set; }
        IRouter Router { get; set; }
        ISet<IPacket> WaitedPackets { get; set; }
        IDataTransformer DataTransformer { get; set; }

        void TickInput();
        void TickMiddle();
        void TickOutput();
    }
}