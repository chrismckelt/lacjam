using System;
using System.Data.SqlClient;

namespace Lacjam.Framework.Exceptions
{
    public class UniqueKeyException : Exception
    {
        public UniqueKeyException(string message, SqlException sqlException)  :base(message,sqlException)
        {
        }
    }
}