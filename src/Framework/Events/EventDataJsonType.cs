using System;
using System.Data;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Lacjam.Framework.Serialization;

namespace Lacjam.Framework.Events
{
    public class EventDataJsonType : IUserType
    {
        public SqlType[] SqlTypes
        {
            get
            {
                var types = new SqlType[2];
                types[0] = new SqlType(DbType.String);
                types[1] = new SqlType(DbType.String, Int32.MaxValue);
                return types;
            }
        }

        public Type ReturnedType
        {
            get { return typeof(EventData); }
        }

        public new bool Equals(object x, object y)
        {
            return x != null && x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var eventTypeName = (string)NHibernateUtil.String.NullSafeGet(rs, names[0]);
            var eventDataSerialized = (string)NHibernateUtil.String.NullSafeGet(rs, names[1]);
            var eventType = Type.GetType(eventTypeName);

            return new EventData(eventTypeName, GetEventData(eventDataSerialized, eventType));
        }

        private static IEvent GetEventData(string eventDataSerialized, Type eventType)
        {
            try
            {
                 return(IEvent)JsonConvert.DeserializeObject(eventDataSerialized, eventType, MetaStoreSerializerSettings.Instance);
            }
            catch (Exception e)
            {
                throw new EventSerializationException("Error deserializing event", e);
            }
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
                return;
            }

            var eventData = (EventData) value;
            var dynamicEvent = eventData.Event as DynamicEvent;

            var eventDataSerialized = dynamicEvent != null ? dynamicEvent.Json : JsonConvert.SerializeObject(eventData.Event, MetaStoreSerializerSettings.Instance);

            NHibernateUtil.String.NullSafeSet(cmd, eventData.Type, index);
            NHibernateUtil.String.NullSafeSet(cmd, eventDataSerialized, index + 1);
        }

        public object DeepCopy(object value)
        {
            if (value == null) return null;
            var eventData = (EventData)value;
            return new EventData(eventData.Type,eventData.Event);
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            //Used for caching, as our object is immutable we can just return it as is
            return cached;
        }

        public object Disassemble(object value)
        {
            //Used for casching, as our object is immutable we can just return it as is
            return value;
        }

        
    }
}