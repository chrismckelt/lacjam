using System;
using System.Diagnostics;

namespace Lacjam.Framework.Utilities
{
    public static class EventLoop
    {

        [DebuggerStepThrough]
        public static void Run(Func<bool> function)
        {
            while (function())
            {
                //System.Threading.Thread.Sleep(5000);
            }
        }
    }
}