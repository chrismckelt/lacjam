using System;
using System.Diagnostics;

namespace Lacjam.Framework.Utilities
{

    public static class FinallyGuarded
    {
        [DebuggerStepThrough]
        public static void Apply(Action guardedBlock, Action finallyBlock)
        {
            try
            {
                guardedBlock();
            }
            finally
            {
                finallyBlock();
            }
        }

         [DebuggerStepThrough]
        public static void Apply(Action guardedBlock,Action<Exception> handleException, Action finallyBlock)
        {
            try
            {
                guardedBlock();
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
            finally
            {
                finallyBlock();
            }
        }
    }

}
