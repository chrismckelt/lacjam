namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public abstract class SelectionDataType : DataType, ISelectionDataType
    {
        public bool AllowMultipleSelection { get; protected set; }
    }
}