using System;
using Lacjam.Core.Domain.MetadataDefinitionGroups.Commands;
using Lacjam.Framework.FP;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups
{
    public class MetadataDefinitionGroupService : IMetadataDefinitionGroupService
    {
        public MetadataDefinitionGroupService(IRepository<MetadataDefinitionGroup> repository)
        {
            _repository = repository;
        }

        public void Create(Guid conceptIdentity, string name, string description, MetadataBag bag)
        {
            var concept = new MetadataDefinitionGroup(conceptIdentity, new MetadataDefinitionGroupName(name), new MetadataDefinitionGroupDescription(description));
            bag.Selection.ForEach(concept.AssociateAttribute);
            _repository.Save(concept.ToMaybe());
        }

        private readonly IRepository<MetadataDefinitionGroup> _repository;
    }
}
