
using MatterCore;

namespace MtrCore.Interfaces
{
    public interface IPortFactory
    {
        IInPort CreateInputPort();
        IOutPort CreateOutputPort();
    }
}
