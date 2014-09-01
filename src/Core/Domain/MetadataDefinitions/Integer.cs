namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class Integer : RegexDataType
    {
        public Integer()
        {
            Tag = "Integer";
            Regex = @"^(\+|-)?\d+$";
        }
    }
}