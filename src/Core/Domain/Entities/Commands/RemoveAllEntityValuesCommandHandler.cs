using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class RemoveAllEntityValuesCommandHandler : ICommandHandler<RemoveAllEntityValuesCommand>
    {

        public RemoveAllEntityValuesCommandHandler(IRepository<Entity> repository)
        {
            _repository = repository;
        }

        public void Handle(RemoveAllEntityValuesCommand command)
        {
            var aggregrate = _repository.Get(command.AggregateIdentity);
            aggregrate.Foreach(x => x.Clear());

            _repository.Save(aggregrate);
        }

        private readonly IRepository<Entity> _repository;
    }
}