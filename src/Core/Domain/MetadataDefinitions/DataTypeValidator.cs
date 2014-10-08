using System;
using System.Text.RegularExpressions;

namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public static class DataTypeValidator
    {
        public static void ValidateValueAgainst(string value, string regex)
        {
            if (string.IsNullOrEmpty(regex) || string.IsNullOrEmpty(value))
                return;

            if (!Regex.IsMatch(value, regex))
                throw new SuppliedValueInvalidFormatException();

        }
    }
}