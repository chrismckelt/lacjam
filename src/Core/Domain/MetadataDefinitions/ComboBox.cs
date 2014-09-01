namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class ComboBox : SelectionDataType
    {
        public ComboBox()
        {
            Tag = "ComboBox";
            AllowMultipleSelection = true;
        }
    }
}