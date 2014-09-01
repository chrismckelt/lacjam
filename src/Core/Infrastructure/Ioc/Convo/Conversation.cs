using System;
using NHibernate;
using uNhAddIns.SessionEasier;
using uNhAddIns.SessionEasier.Conversations;

namespace Lacjam.Core.Infrastructure.Ioc.Convo
{

    [Serializable]
    public class Conversation : NhConversation, IConversation,IDisposable
    {
        public const string ReadOnly = "ReadOnly_";
        private readonly ISessionFactoryProvider _provider;
        private readonly ISessionWrapper _wrapper;
       


        public Conversation(ISessionFactoryProvider provider, ISessionWrapper wrapper, string conversationId) : base(provider, wrapper, conversationId)
        {
            _provider = provider;
            _wrapper = wrapper;
            
        }

        protected override ISession Wrap(ISession session)
        {
            if (!Id.StartsWith(Conversation.ReadOnly) && _wrapper.IsWrapped(session))
                return base.Wrap(session);
            return session;
        }

    }
    }
