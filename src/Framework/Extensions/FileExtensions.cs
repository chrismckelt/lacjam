using System;
using System.IO;
using System.Linq;
using Lacjam.Framework.Utilities;

namespace Lacjam.Framework.Extensions
{
    public static class FileExtensions
    {
        public static string ToJavaScriptExtension(this string str, bool justdoIt = false)
        {
            if (string.IsNullOrEmpty(str)) return null;

            if (Path.HasExtension(str) && str.IsFileExtensionValid() && !justdoIt)
            {
                return Path.ChangeExtension(str, Constant.FileExtensions.Js);
            }
            else
            {
                return str + Constant.FileExtensions.Js;
            }
        }


        /// <summary>
        /// Checks if file extension passed as a parameter is 
        /// valid. The expected format in order for it to be valid 
        /// is ".listofCharacters", that is, a dot followed by a 
        /// set of valid file name characters. The method does not 
        /// limit the file extension to a maximum number of 
        /// characters.
        /// </summary>
        /// <param name="fExt">File extension to be 
        /// tested for format validity.</param>
        /// <returns><b>True</b> if the file extension passed is 
        /// valid and <b>false</b> otherwise.</returns>
        public static bool IsFileExtensionValid(this string fExt)
        {
            bool answer = true;
            if (!String.IsNullOrWhiteSpace(fExt)
                && fExt.Length > 1
                && fExt[0] == '.')
            {
                char[] invalidFileChars = Path.GetInvalidFileNameChars();
                foreach (char c in invalidFileChars)
                {
                    if (fExt.Contains(c))
                    {
                        answer = false;
                        break;
                    }
                }
            }
            return answer;
        }
    }
}
