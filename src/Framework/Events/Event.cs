using System;

namespace Lacjam.Framework.Events
{
    public abstract class Event : IEvent
    {
        protected Event()
        {
        }

        protected Event(Guid aggregateIdentity)  // all childrens contructors must have same variable name for JSON Serialization to work
        {
            AggregateIdentity = aggregateIdentity;
        }

        public Guid AggregateIdentity
        {
            get;
            set;
        }
    }



}
