using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Lacjam.Framework.Extensions;

namespace Lacjam.Framework.Utilities
{

    public static class Util
    {
        public static int CalculateAge(DateTime? dob)
        {
            if (dob.HasValue && dob.Value != DateTime.MinValue)
            {
                //Calculate from current day
                DateTime now = DateTime.Today;

                //Get difference in years
                int years = now.Year - dob.Value.Year;
                // subtract another year if we're before the
                // birth day in the current year
                if (now.Month < dob.Value.Month || (now.Month == dob.Value.Month && now.Day
                                                    < dob.Value.Day))
                    --years;

                return years;
            }

            return -1;
        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            bool result = false;
            if (String.IsNullOrEmpty(email))
                return result;
            email = email.Trim();
            result = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return result;
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = String.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
                return str.Substring(0, maxLength);
            else
                return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return String.Empty;

            return str;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            bool result = false;
            Array.ForEach(stringsToValidate, str =>
            {
                if (String.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }

       

        public static T AsType<T>(object item)
        {
            return (T) item;
        }



        [DebuggerStepThrough]
        public static void EnsureSiteIsUp(string url)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          using (var client = new WebClient())
                                          {
                                              //manipulate request headers (optional)
                                              client.Headers.Add(HttpRequestHeader.UserAgent,
                                                                 "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                                              Console.WriteLine(url);
                                              Debug.WriteLine(url);
                                              //execute request and read response as string to console
                                              if (url != null)
                                                  using (var reader = new StreamReader(client.OpenRead(url)))
                                                  {
                                                      //string s = reader.ReadToEnd();
                                                      //Console.WriteLine(s);
                                                      Console.WriteLine("Checked site is up " + url);
                                                  }
                                          }
                                      });
        }

        public static string ConvertThreeLetterNameToTwoLetterName(string name)
        {
            if (name.Length != 3)
            {
                throw new ArgumentException("name must be three letters.");
            }

            name = name.ToUpper();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);
                if (region.ThreeLetterISORegionName.ToUpper() == name)
                {
                    return region.TwoLetterISORegionName;
                }
            }

            return null;
        }

        public static string ConvertCountryNameTo3LetterIsoCode(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);
                if (region.EnglishName.ToUpper() == name.ToUpper())
                {
                    return region.ThreeLetterISORegionName;
                }
            }

            return null;
        }


        /// <summary>
        /// assumes date2 is the bigger date for simplicity
        /// 
        /// could use System.Data.Linq.SqlClient.SqlMethods.DateDiffMonth(startDT, endDT);
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="Years"></param>
        /// <param name="Months"></param>
        /// <param name="Weeks"></param>
        /// <param name="Days"></param>
        public static void GetDateDifference(DateTime date1, DateTime date2, out int Years, out int Months, out int Weeks, out int Days)
        {
            //assumes date2 is the bigger date for simplicity

            //years
            TimeSpan diff = date2 - date1;
            Years = diff.Days / 366;
            DateTime workingDate = date1.AddYears(Years);

            while (workingDate.AddYears(1) <= date2)
            {
                workingDate = workingDate.AddYears(1);
                Years++;
            }

            //months
            diff = date2 - workingDate;
            Months = diff.Days / 31;
            workingDate = workingDate.AddMonths(Months);

            while (workingDate.AddMonths(1) <= date2)
            {
                workingDate = workingDate.AddMonths(1);
                Months++;
            }

            //weeks and days
            diff = date2 - workingDate;
            Weeks = diff.Days / 7; //weeks always have 7 days
            Days = diff.Days % 7;
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }


        public static IList<string> GetConstants(Type type, bool lowerToInvariantWithoutSpaces)
        {
 
            IList<string> list = new List<string>();

            FieldInfo[] fieldInfos = type.GetFields(
                // Gets all public and static fields

                BindingFlags.Public | BindingFlags.Static |
                // This tells it to get the fields from all base types as well

                BindingFlags.FlattenHierarchy);

            // Go through the list and only pick out the constants
            foreach (FieldInfo fi in fieldInfos)
                // IsLiteral determines if its value is written at 
                //   compile time and not changeable
                // IsInitOnly determine if the field can be set 
                //   in the body of the constructor
                // for C# a field which is readonly keyword would have both true 
                //   but a const field would have only IsLiteral equal to true
                if (fi.IsLiteral && !fi.IsInitOnly)
                {
                    string result = Convert.ToString(fi.GetValue(null));

                    list.Add(lowerToInvariantWithoutSpaces ? result.ToLowerInvariantWithOutSpaces() : result);
                }

            return list;
        }

        public static byte[] ConvertStreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static X509Certificate2 GetCertificate(StoreName name, StoreLocation location, string friendlyName)
        {
            X509Store store = new X509Store(name, location);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);

            try
            {
                X509Certificate2 result = null;

                //
                // Every time we call store.Certificates property, a new collection will be returned.
                //
                certificates = store.Certificates;

                for (int i = 0; i < certificates.Count; i++)
                {
                    X509Certificate2 cert = certificates[i];

                    if (cert.FriendlyName.ToLower() == friendlyName.ToLower())
                    {
                        if (result != null)
                        {
                            throw new ApplicationException(String.Format("There are multiple certificate for Name {0}", friendlyName));
                        }

                        result = new X509Certificate2(cert);
                    }
                }

                if (result == null)
                {
                    throw new ApplicationException(String.Format("No certificate was found for Name {0}", friendlyName));
                }

                return result;
            }
            finally
            {
                if (certificates != null)
                {
                    for (int i = 0; i < certificates.Count; i++)
                    {
                        X509Certificate2 cert = certificates[i];
                        cert.Reset();
                    }
                }

                store.Close();
            }
        }


        private static Dictionary<string, Type> _typeCache = new Dictionary<string, Type>();

        public static bool TryFindType(string typeName, out Type t) {
            lock (_typeCache)
            {
                if (!_typeCache.TryGetValue(typeName, out t))
                {
                    foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
                        t = a.GetType(typeName);
                        if (t != null)
                            break;
                    }
                    _typeCache[typeName] = t; // perhaps null
                }
            }
            return t != null;
        }

        public static string[] SplitFullName(string fullName)
        {
            if (String.IsNullOrWhiteSpace(fullName))
                return new[] { "", "" };
            fullName = fullName.Trim();
            var lastSpace = fullName.LastIndexOf(' ');
            if (lastSpace < 0)
                return new[] { "", fullName };
            return new[] { fullName.Substring(0, lastSpace), fullName.Substring(lastSpace + 1) };
        }

        public static int GetAgeFromDateOfBirth(DateTime? dateOfBirth)
        {
            return GetAgeFromDateOfBirth(dateOfBirth, DateTime.Today);
        }

        public static int GetAgeFromDateOfBirth(DateTime? dateOfBirth, DateTime? asAtDate)
        {
            if (dateOfBirth == null)
                return 0;
            if (asAtDate == null)
                asAtDate = DateTime.Today;
            var age = asAtDate.Value.Year - dateOfBirth.Value.Year;
            if (dateOfBirth > asAtDate.Value.AddYears(-age)) age--;
            return age;
        }

        public static DateTime GetNextBirthdayFromDateOfBirth(DateTime dateOfBirth)
        {
            var nextBirthday = dateOfBirth.AddYears(DateTime.Today.Year - dateOfBirth.Year);
            if (nextBirthday <= DateTime.Today) // if birthday already occurred this year
                nextBirthday = nextBirthday.AddYears(1);
            return nextBirthday;
        }


        /// <summary>
        /// NB:// this must be the SAME in the STS project - fixed in an emergency release
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password)
        {
            const string symbolsDisallowed = "<>";
            if (password.Length < 8) return false;
            var categories = new[] { @"\d", "[A-Z]", "[a-z]", @"[^\dA-Za-z" + symbolsDisallowed + "]" };
            var matchCount = categories.Count(c => Regex.IsMatch(password, c));
            return matchCount >= 3 && !Regex.IsMatch(password, @"[" + symbolsDisallowed + "]");
        }

        public static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        public static IEnumerable<string> SplitKeepingWords(string str, int length)
        {
            if (str == null)
                yield break;

            string chunk;
            int current = 0;
            int lastSep = -1;
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsSeparator(str[i]))
                {
                    lastSep = i;
                    continue;
                }

                if ((i - current) >= length)
                {
                    if (lastSep < 0) // big first word case
                        continue;

                    chunk = str.Substring(current, lastSep - current).Trim();
                    if (chunk.Length > 0)
                        yield return chunk;

                    current = lastSep;
                }
            }
            chunk = str.Substring(current).Trim();
            if (chunk.Length > 0)
                yield return chunk;
        }

      
    }

}
