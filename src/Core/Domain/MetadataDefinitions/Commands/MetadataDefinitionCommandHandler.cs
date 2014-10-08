using System.Linq;
using Lacjam.Framework.Extensions;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.MetadataDefinitions.Commands
{
    public class MetadataDefinitionCommandHandler :
        ICommandHandler<CreateMetadataDefinitionCommand>,
        ICommandHandler<DeleteMetadataDefinitionCommand>,
        ICommandHandler<ReLabelMetadataDefinitionCommand>,
         ICommandHandler<ReLabelMetadataDefinitionDescriptionCommand>,
        ICommandHandler<UpdateMetadataDefinitionRegexCommand>,
        ICommandHandler<UpdateMetadataDefinitionValuesCommand>,
        ICommandHandler<ChangeMetadataDefinitionDataTypeCommand>
    {

        public MetadataDefinitionCommandHandler(IRepository<MetadataDefinition> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateMetadataDefinitionCommand command)
        {
            var definition = new MetadataDefinition(command.Identity, new MetadataDefinitionName(command.Name), new MetadataDefinitionDescription(command.Description), DataTypeBuilder.Create(command.DataType), command.Regex);

            if (command.Values != null && command.Values.Any())
                command.Values.Each(x => definition.AddAllowableValue(new AllowableValue(x)));

            _repository.Save(definition.ToMaybe());
        }

        public void Handle(DeleteMetadataDefinitionCommand command)
        {
            var definition = _repository.Get(command.Identity);
            definition.Foreach(x => x.Delete());
            _repository.Save(definition);
        }

        public void Handle(ReLabelMetadataDefinitionCommand command)
        {
            var definition = _repository.Get(command.Identity);
            definition.Foreach(x => x.ReLabel(command.Name));
            _repository.Save(definition);
        }

        public void Handle(ReLabelMetadataDefinitionDescriptionCommand command)
        {
            var definition = _repository.Get(command.Identity);
            definition.Foreach(x => x.ReLabel(command.Description));
            _repository.Save(definition);
        }

        public void Handle(UpdateMetadataDefinitionRegexCommand command)
        {
            var definition = _repository.Get(command.Identity);
            definition.Foreach(x => x.ChangeRegularExpression(command.Regex));
            _repository.Save(definition);
        }

        public void Handle(ChangeMetadataDefinitionDataTypeCommand command)
        {
            var definition = _repository.Get(command.Identity);
            definition.Foreach(x => x.ChangeDataType(command.DataType));
            _repository.Save(definition);
        }

        public void Handle(UpdateMetadataDefinitionValuesCommand command)
        {
            var definition = _repository.Get(command.Identity);
            definition.Foreach(x =>
            {
                x.ClearValues();
                command.Values.Each(v => x.AddAllowableValue( new AllowableValue(v) ));
            });
            _repository.Save(definition);
        }

        private readonly IRepository<MetadataDefinition> _repository;
    }

}