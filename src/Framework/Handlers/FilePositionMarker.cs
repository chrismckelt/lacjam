using System.IO;

namespace Lacjam.Framework.Handlers
{
    public class FilePositionMarker : IEventStorePositionMarker
    {
        public void Mark<T>(T position)
        {
            File.WriteAllText(filename, position.ToString());
        }

        private const string filename = "stream-position";
    }

}
