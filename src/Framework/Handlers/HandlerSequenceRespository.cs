using NHibernate;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public class HandlerSequenceRespository : IHandlerSequenceRespository
    {
        private readonly ISessionFactory _sessionFactory;


        public HandlerSequenceRespository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private static HandlerSequence GetHandlerSequence(ISessionFactory sessionFactory, string name)
        {
            var session = sessionFactory.GetCurrentSession();
            var sequence = session.Get<HandlerSequence>(name);
            if (sequence != null) 
                return sequence;

            sequence = new HandlerSequence(name);
            session.Save(sequence);
            return sequence;
        }

        public void Save(long eventSequence, string name)
        {
            var handlerSequence = GetHandlerSequence(_sessionFactory, name);
            handlerSequence.UpdateSequence(eventSequence);
            _sessionFactory.GetCurrentSession().Update(handlerSequence);
        }

        public IMaybe<long> GetEventSequence(string name)
        {
            var handlerSequence = GetHandlerSequence(_sessionFactory, name);
            return handlerSequence == null ? new None<long>() : handlerSequence.Seq.ToMaybe();
        }

    }
}