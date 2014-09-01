using System;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.Attribute.Commands
{
    public class AttributeCommandHandler : ICommandHandler<CreateAttributeCommand>, ICommandHandler<DeleteAttributeCommand>, ICommandHandler<UpdateAttributeCommand>
    {
        
        public AttributeCommandHandler(IRepository<Attribute> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateAttributeCommand command)
        {
            var attribute = new Attribute(command.Identity, new AttributeName(command.AttributeName), false);
            _repository.Save(attribute.ToMaybe());
        }

        public void Handle(DeleteAttributeCommand command)
        {
            var attribute = _repository.Get(command.Identity);
            attribute.Foreach(x => x.Delete());
            _repository.Save(attribute);
        }

        public void Handle(UpdateAttributeCommand command)
        {
            var attribute = _repository.Get(command.Identity);
            attribute.Foreach(x => x.ReLabel(new AttributeName(command.AttributeName)));
            _repository.Save(attribute);
        }

        private readonly IRepository<Attribute> _repository;
    }
    
}