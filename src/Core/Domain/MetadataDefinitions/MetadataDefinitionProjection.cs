using System;
using Lacjam.Core.Infrastructure;

namespace Lacjam.Core.Domain.MetadataDefinitions
{

    public class MetadataDefinitionProjection
    {

        public MetadataDefinitionProjection() { }

        public MetadataDefinitionProjection(Guid identity, string name,string description, string datatype, string regex, TrackingBase tracking)
        {
            Identity = identity;
            Name = name;
            Description = description;
            DataType = datatype;
            Regex = regex;
            Tracking = tracking;
        }

        public MetadataDefinitionProjection(Guid identity, string name, string datatype, string regex)
            : this(identity, name,string.Empty, datatype, regex, new TrackingBase())
        {
        }

        public MetadataDefinitionProjection(Guid identity, string name, string datatype)
            : this(identity, name, datatype, String.Empty)
        {
        }

        public virtual MetadataDefinitionProjection WithNewRegularExpression(string regex)
        {
            var result = new MetadataDefinitionProjection(Identity, Name, Description, DataType, regex, Tracking);
            result.SetUpdated();
            return result;
        }

        public virtual MetadataDefinitionProjection WithNewDetails(string name)
        {
            var result = new MetadataDefinitionProjection(Identity, name, Description, DataType, Regex, Tracking);
            result.SetUpdated();
            return result;
        }

        public virtual MetadataDefinitionProjection WithNewDescription(string desc)
        {
            var result = new MetadataDefinitionProjection(Identity, Name, desc, DataType, Regex, Tracking);
            result.SetUpdated();
            return result;
        }

        public virtual MetadataDefinitionProjection WithDataType(string datatype)
        {
            var result = new MetadataDefinitionProjection(Identity, Name, datatype, Regex);
            result.SetUpdated();
            return result;
        }

        private void SetUpdated()
        {
            if (Tracking != null)
                Tracking.LastModifiedUtcDate = DateTime.UtcNow;
            else
                Tracking = new TrackingBase();
        }

        public virtual void Delete()
        {
            SetUpdated();
            Tracking.IsDeleted = true;
        }

        public virtual Guid Identity { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string DataType { get; set; }
        public virtual string Regex { get; set; }
        public virtual TrackingBase Tracking { get; set; }
    }
}
