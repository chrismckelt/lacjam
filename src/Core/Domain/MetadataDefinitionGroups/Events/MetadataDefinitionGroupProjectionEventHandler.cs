using System;
using System.Linq;
using System.Linq.Expressions;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Events;
using Lacjam.Framework.FP;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Projection;

namespace Lacjam.Core.Domain.MetadataDefinitionGroups.Events
{

    public class MetadataDefinitionGroupProjectionEventHandler : 
        IEventHandler<MetadataDefinitionGroupCreatedEvent>,
        IEventHandler<MetadataDefinitionGroupDeletedEvent>, 
        IEventHandler<MetadataDefinitionGroupDescriptionChangedEvent>,
        IEventHandler<MetadataDefinitionGroupNameChangedEvent>
    {
        public MetadataDefinitionGroupProjectionEventHandler(IReadStoreRepository<MetadataDefinitionGroupProjection> repository)
        {
            _repository = repository;
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionGroupCreatedEvent @event)
        {
            var readmodel = new MetadataDefinitionGroupProjection
            {
                Identity = @event.AggregateIdentity,
                Name = @event.Name.Name,
                Description = @event.Description.Description,
                Tracking = new TrackingBase()
            };

            _repository.Save(readmodel.ToMaybe());
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionGroupDeletedEvent @event)
        {
            var projection = from p in _repository.Get(@event.AggregateIdentity)    
                             select p;

            _repository.Remove(projection);
        }

        private static Expression<Func<TrackingBase,Func<TrackingBase>>> SetTracking(TrackingBase trk)
        {
            return (x) => SetDeleted(trk);
        }

        public static Func<TrackingBase> SetDeleted(TrackingBase tb)
        {
            return () =>
            {
                tb.IsDeleted = true;
                return tb;
            };
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionGroupDescriptionChangedEvent @event)
        {
            var projection = from p in _repository.Get(@event.AggregateIdentity)
                             select p.WithNewDescription(@event.Description);
           
            _repository.Update(projection);
        }

        [ImmediateDispatch]
        public void Handle(MetadataDefinitionGroupNameChangedEvent @event)
        {
            var projection = from p in _repository.Get(@event.AggregateIdentity)
                             select p.WithNewName(@event.Name);

            _repository.Update(projection);
        }

        private readonly IReadStoreRepository<MetadataDefinitionGroupProjection> _repository;
    }
}
