namespace Lacjam.Framework.Handlers
{

    public interface IEventStorePositionMarker
    {
        void Mark<T>(T position);
    }

}
