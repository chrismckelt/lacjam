using Lacjam.Framework.Handlers;
using Lacjam.Framework.Model;

namespace Lacjam.Core.Domain.Entities.Commands
{
    public class DeleteEntityCommandHandler : ICommandHandler<DeleteEntityCommand>
    {

        public DeleteEntityCommandHandler(IRepository<Entity> repository)
        {
            _repository = repository;
        }

        public void Handle(DeleteEntityCommand command)
        {
            var aggregrate = _repository.Get(command.Identity);
            aggregrate.Foreach(x => x.Delete());
            _repository.Save(aggregrate);
        }

        private readonly IRepository<Entity> _repository;
    }
}