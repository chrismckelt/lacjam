namespace Lacjam.Core.Domain.Entities
{
    public class SuppliedValue
    {
        protected SuppliedValue(){}

        public SuppliedValue(string content)
        {
            Content = content;
        }

        public string Content { get; protected set; }
    }
}