namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public abstract class RegexDataType : DataType, IRegexDataType
    {
        public string Regex { get; protected set; }
    }
}