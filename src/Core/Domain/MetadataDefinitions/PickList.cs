namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public class PickList : SelectionDataType
    {
        public PickList()
        {
            Tag = "PickList";
            AllowMultipleSelection = false;
        }
    }
}