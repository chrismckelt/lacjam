namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class RegexDataType : DataType, IRegexDataType
    {
        public RegexDataType()
        {
        }

        public RegexDataType(string regex)
        {
            Regex = regex;
        }

        public string Regex { get; protected set; }
    }
}