using Lacjam.Framework.Extensions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class MetadataDefinitionCommandHandler : 
        ICommandHandler<CreateMetadataDefinitionCommand>, 
        ICommandHandler<DeleteMetadataDefinitionCommand>, 
        ICommandHandler<UpdateMetadataDefinitionCommand>
    {
        
        public MetadataDefinitionCommandHandler(IRepository<MetadataDefinition> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateMetadataDefinitionCommand command)
        {
            var dataType = DataTypeBuilder.Create(command.DataType);
            var attribute = new MetadataDefinition(command.Identity, new MetadataDefinitionName(command.Name), dataType, command.Regex);
            command.Values.Each(x => attribute.AddAllowableValue(new AllowableValue(x)));

            _repository.Save(attribute.ToMaybe());
        }

        public void Handle(DeleteMetadataDefinitionCommand command)
        {
            var attribute = _repository.Get(command.Identity);
            attribute.Foreach(x => x.Delete());
            _repository.Save(attribute);
        }

        public void Handle(UpdateMetadataDefinitionCommand command)
        {
            var attribute = _repository.Get(command.Identity);
            attribute.Foreach(x => x.ReLabel(new MetadataDefinitionName(command.Name)));
            _repository.Save(attribute);
        }

        private readonly IRepository<MetadataDefinition> _repository;
    }
    
}