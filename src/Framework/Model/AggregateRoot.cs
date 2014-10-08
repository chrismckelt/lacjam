using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Lacjam.Framework.Events;
using Lacjam.Framework.Extensions;

namespace Lacjam.Framework.Model
{
    public abstract class AggregateRoot<TState> : Entity, IAggregateRoot
        where TState : class
    {
        protected AggregateRoot()
        {
            Version = 0;
            _events = new List<IEvent>();
            State = GetState();
        }

        private static TState GetState()
        {
            var ctor = typeof(TState).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,null, new Type[]{},null );
            if (ctor != null) return ctor.Invoke(new object[]{}).As<TState>();

            return default(TState);
        }

        protected AggregateRoot(Guid identity, IEnumerable<IEvent> events)
            : base(identity)
        {
            Version = 0;
            _events = new List<IEvent>();
            State = GetState();
            ApplyChanges(events);
        }

        protected void ApplyIdentity(Guid identity)
        {
            Identity = identity;
        }

        public int GetVersion()
        {
            return Version;
        }

        public void MarkChangesCommited()
        {
            Version += _events.Count();
            _events.Clear();
        }

        protected void ApplyChanges(IEnumerable<IEvent> set)
        {
            foreach (var @event in set)
                ApplyChange(@event, false);
        }

        private void ApplyChange(IEvent @event, bool commit)
        {

            var dispatchMethod = this.GetType()
                .GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] {@event.GetType()},
                    null);

            dispatchMethod.Invoke(this, new object[]{ @event});

            if (commit)
                _events.Add(@event);
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        public IEnumerable<IEvent> GetUncommitedChanges()
        {
            return new ReadOnlyCollection<IEvent>(_events);
        }

        protected TState State { get; set; }
        protected int Version;
        protected readonly List<IEvent> _events;
    }

}
