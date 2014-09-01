namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public interface ISelectionDataType : IDataType
    {
        bool AllowMultipleSelection { get; }
    }
}