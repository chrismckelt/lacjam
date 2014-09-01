namespace Lacjam.Core.Domain.MetadataDefinitions
{

    public class AllowableValue
    {
        public AllowableValue(string value)
        {
            _value = value;
        }

        public string GetValue()
        {
            return _value;
        }

        private string _value;
    }

}
