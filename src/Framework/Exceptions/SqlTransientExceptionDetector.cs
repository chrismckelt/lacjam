using System;
using System.Data.SqlClient;
using System.Linq;

namespace Lacjam.Framework.Exceptions
{
    public class SqlTransientExceptionDetector
    {
        public static bool IsTransient(Exception exception)
        {
            if (exception == null)
                return false;

            if (!(exception is SqlException))
                return false;

            if (TransientErrors.Any(x => x == ((SqlException) exception).ErrorCode))
                return true;

            return false;

        }

        private readonly static int[] TransientErrors =
        { 
            1608,  // A network error was encountered while sending results to the front end
            10008,  // Bad token from SQL Server: datastream processing out of sync
            10010,  // Read from SQL Server failed
            10018,  // Error closing network connection
            10025,  // Write to SQL Server failed
            10053,  // Software caused connection abort
            10054,  // Connection reset by peer
            10058,  // Can't send after socket shutdown
            17824,  // Unable to write to server-side connection
            17825,  // Unable to close server-side connection
            17832,  // Unable to read login packet(s)            
        };
    }
}