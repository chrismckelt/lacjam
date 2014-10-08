using Lacjam.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TypeLite;

namespace Lacjam.WebApi.Controllers.MetadataDefinition
{
    [TsClass]
    public class MetadataDefinitionResource
    {

        public bool NameMatches(MetadataDefinitionResource other)
        {
            if (other == null)
                return false;

            return Equals(Name, other.Name);
        }

        public bool DescriptionMatches(MetadataDefinitionResource other)
        {
            if (other == null)
                return false;

            return Equals(Description, other.Description);
        }

        public bool DataTypeMatches(MetadataDefinitionResource other)
        {
            if (other == null)
                return false;

            return Equals(DataType, other.DataType);
        }

        public bool RegexMatches(MetadataDefinitionResource other)
        {
            if (other == null)
                return false;

            return Equals(Regex, other.Regex);
        }

        public bool ValuesMatch(MetadataDefinitionResource other)
        {
            if (other == null)
                return false;

            if (Values == null)
                return other.Values == null || !other.Values.Any();

            if (other.Values == null)
                return Values == null || !Values.Any();

            if (Values.Count != other.Values.Count) return false;

            return Values.Intersect(other.Values).Count() == Values.Count;
        }

        public Guid Identity { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters ")]
        public string Name { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters ")]
        public string Description { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "DataType cannot exceed 200 charactors ")]
        public string DataType { get; set; }
        [StringLength(1000, ErrorMessage = "Regex cannot exceed 1000 characters ")]
        public string Regex { get; set; }
        public HashSet<string> Values { get; set; }
        public TrackingBase Tracking { get; set; }
    }
}