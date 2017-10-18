namespace MatterCore
{
    public interface IQueue<T>
    {
        void Enqueue(T obj);
        T Dequeue();
    }
}