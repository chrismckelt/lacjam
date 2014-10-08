namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class Text : RegexDataType
    {
        public Text(string regex) : this()
        {
            Regex = regex;
        }

        public Text()
        {
            Tag = "Text";
            Regex = ".*";
        }
    }
}