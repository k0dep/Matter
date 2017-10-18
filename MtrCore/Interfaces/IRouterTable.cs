namespace MatterCore
{
    public interface IRouterTable
    {
        IRoute GetRoute(int label);
        void AddRoute(int label, IRoute record);
    }
}