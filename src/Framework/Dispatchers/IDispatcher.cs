namespace Lacjam.Framework.Dispatchers
{
    public interface IDispatcher
    {
        void Start(string mode);
        void Stop();
    }
}