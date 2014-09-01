using System;

namespace Lacjam.Framework.Utilities
{
    public static class Uuid
    {
        /// <summary>
        /// Generate a GUID that is
        ///   1. Cryptographically random
        ///   2. SQL Server index friendly
        ///   3. Convertible to Filemaker UUID format
        /// </summary>
        public static Guid NewGuid()
        {
            // https://github.com/nhibernate/nhibernate-core/blob/master/src/NHibernate/Id/GuidCombGenerator.cs
            var guidArray = Guid.NewGuid().ToByteArray();

            var baseDate = new System.DateTime(1900, 1, 1);
            var now = System.DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            var msecs = now.TimeOfDay;

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return EnsureConvertible(guidArray);
        }

        private static Guid EnsureConvertible(byte[] guidRaw)
        {
            var upper = BitConverter.ToUInt64(guidRaw, 0);
            var lower = BitConverter.ToUInt64(guidRaw, 8);

            var s = lower & 0xFFFFFF;
            var t = (lower & 0x1FFFFFFFFF000000) >> 24;
            var c = ((lower & 0xE000000000000000) >> 61) +
                    ((upper & 0x3FFF) << 3);

            var n = (upper & 0xFFFFFFFFFFFFC000) >> 14;

            // mask bits if conversion to FM uuid format will overflow
            
            // nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnncccccccccccccc
            // ccctttttttttttttttttttttttttttttttttttttssssssssssssssssssssssss
            
            if (t >= 100000000000)
            {
                lower = lower & 0xEFFFFFFFFFFFFFFF;
            }

            if (s >= 10000000)
            {
                lower = lower & 0xFFFFFFFFFF7FFFFF;
            }
            if (c >= 100000)
            {
                upper = upper & 0xFFFFFFFFFFFFDFFF;
            }
            if (n > 100000000000000)
            {
                upper = upper & 0x7FFFFFFFFFFFFFFF;
            }

            return PairToGuid(upper, lower);
        }

        /// <summary>
        /// Convert a Filemaker UUID to a .NET GUID
        /// https://github.com/jbante/FileMaker-Techniques/blob/master/CustomFunctions/UUID/UUIDTimeNIC.fmfn
        ///                 A 41-digit delimited number of the form:
        ///  *                v-r-tttttttttttt-sssssss-ccccc-nnnnnnnnnnnnnnn
        ///  *                The sections of the UUID correspond to:
        ///  *                v: A UUID version (type) number
        ///  *                r: A variant code reserved by the RFC 4122 standard
        ///  *                t: The creation timestamp (seconds since 0001-01-01T00:00:00)
        ///  *                s: A serial number, reset for each second
        ///  *                c: A session key, randomly generated for each session
        ///  *                n: The NIC address ("node") of the device that created the UUID
        /// </summary>
        public static Guid ToGuid(this string uuid)
        {

            if (uuid != null && uuid == Guid.Empty.ToString())
                return Guid.Empty;

            if (String.IsNullOrWhiteSpace(uuid))
                return Guid.Empty;

            Guid g = Guid.Empty;
            var alreadyIsAGuid = (Guid.TryParse(uuid, out g));
            if (alreadyIsAGuid) return g;

            var parts = uuid.Split('-');
            if (parts.Length != 6)
            {
                throw new ArgumentException("Expected 6 segments.");
            }
            var t = UInt64.Parse(parts[2]) & 0x1FFFFFFFFF;    // could be 12 digits (40 bits), but actually 11 digits (37 bits... until the year 5000)
            var s = UInt64.Parse(parts[3]) & 0xFFFFFF;        // 7 digits, 24 bits
            var c = UInt64.Parse(parts[4]) & 0x1FFFF;         // 5 digits, 17 bits
            var n = UInt64.Parse(parts[5]) & 0x3FFFFFFFFFFFF; // 15 digits, 50 bits

            // nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnncccccccccccccc
            // ccctttttttttttttttttttttttttttttttttttttssssssssssssssssssssssss

            // all of n, first 14 bits of c
            var upper = (n << 14) + ((c & 0x1FFF8) >> 3);
            // last 3 bits of c, all of t, all of s
            var lower = ((c & 0x7) << 61) + (t << 24) + s;

            return PairToGuid(upper, lower);
        }

        /// <summary>
        /// Convert a GUID to Filemaker UUID format
        /// </summary>
        public static string ToUuid(this Guid guid)
        {
            var guidRaw = guid.ToByteArray();

            var upper = BitConverter.ToUInt64(guidRaw, 0);
            var lower = BitConverter.ToUInt64(guidRaw, 8);

            var s = lower & 0xFFFFFF;
            var t = (lower & 0x1FFFFFFFFF000000) >> 24;
            var c = ((lower & 0xE000000000000000) >> 61) +
                    ((upper & 0x3FFF) << 3);

            var n = (upper & 0xFFFFFFFFFFFFC000) >> 14;

            return string.Format("1-2-{0:D12}-{1:D7}-{2:D5}-{3:D15}", t, s, c, n);
        }

        private static Guid PairToGuid(ulong upper, ulong lower)
        {
            var upperBytes = BitConverter.GetBytes(upper);
            var lowerBytes = BitConverter.GetBytes(lower);

            var guidRaw = new byte[16];

            Buffer.BlockCopy(upperBytes, 0, guidRaw, 0, 8);
            Buffer.BlockCopy(lowerBytes, 0, guidRaw, 8, 8);

            return new Guid(guidRaw);
        }

    }
}