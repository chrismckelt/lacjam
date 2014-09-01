namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class FloatingPoint : RegexDataType
    {
        public FloatingPoint()
        {
            Tag = "FloatingPoint";
            Regex = @"[-+]?[0-9]*\.?[0-9]+";
        }
    }
}