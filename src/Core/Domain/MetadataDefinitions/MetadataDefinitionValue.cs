namespace Lacjam.Core.Domain.MetadataDefinitions
{

    public class AllowableValue
    {

        public AllowableValue(string content)
        {
            Content = content;
        }

        public string GetValue()
        {
            return Content;
        }

        public string Content { get; protected set; }
    }

}
