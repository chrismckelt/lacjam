namespace Lacjam.Framework.Time
{
    public interface IClock
    {
        System.DateTime GetLocalDateTime();
        System.DateTime GetUtcDateTime();
    }

    public sealed class SystemClock : IClock
    {
        public System.DateTime GetLocalDateTime()
        {
            return System.DateTime.Now;
        }

        public System.DateTime GetUtcDateTime()
        {
            return System.DateTime.UtcNow;
        }
    }

    public static class ClockProvider
    {
        private static IClock _clock = new SystemClock();

        public static void SetClock(IClock clock)
        {
            _clock = clock;
        }

        public static IClock Current
        {
            get { return _clock; }
        }
    }


}