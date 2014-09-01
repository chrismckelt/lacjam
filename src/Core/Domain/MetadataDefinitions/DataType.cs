namespace Lacjam.Core.Domain.MetadataDefinitions
{
    public abstract class DataType : IDataType
    {
        public virtual string Tag { get; protected set; }
    }
}