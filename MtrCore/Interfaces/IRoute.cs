namespace MatterCore
{
    public interface IRoute
    {
        void Route(IPacket packet, ICore core);
    }
}