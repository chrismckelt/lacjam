
using System.Data.Entity;
using Lacjam.Framework.FP;

namespace Lacjam.Framework.Handlers
{
    public class HandlerSequenceRespository : IHandlerSequenceRespository
    {
        private readonly DbContext _sessionFactory;


        public HandlerSequenceRespository(DbContext sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private static HandlerSequence GetHandlerSequence(DbContext session, string name)
        {
            ;
            //var sequence = session.Entry()<HandlerSequence>(name);
            ////if (sequence != null) 
            ////    return sequence;

            ////sequence = new HandlerSequence(name);
            ////session.Save(sequence);
            //return sequence;
            return null;
        }

        public void Save(long eventSequence, string name)
        {
            var handlerSequence = GetHandlerSequence(_sessionFactory, name);
            handlerSequence.UpdateSequence(eventSequence);
         //   _sessionFactory.Update(handlerSequence);
        }

        public IMaybe<long> GetEventSequence(string name)
        {
            var handlerSequence = GetHandlerSequence(_sessionFactory, name);
            return handlerSequence == null ? new None<long>() : handlerSequence.Seq.ToMaybe();
        }

    }
}