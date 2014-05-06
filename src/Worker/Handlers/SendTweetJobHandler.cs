using Lacjam.Core;
using Lacjam.Worker.Jobs;
using LinqToTwitter;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Lacjam.Worker.Handlers
{
    public class SendTweetJobHandler : HandlerBase<SendTweetJob>
    {
        public static class ValidationMessages
        {
            public const string NoPayload = "No payload  ";
            public const string MessageGreaterThan140Characters = "Message over 140 chars. Must be undeo long to tweet.";
        }

        public SendTweetJobHandler(IBus bus, Runtime.ILogWriter logger)
            : base(bus, logger)
        {
        }

        public override void Handle(SendTweetJob message)
        {
            if (string.IsNullOrEmpty(message.Payload))
            {
                throw new ValidationException(ValidationMessages.NoPayload + message.ToString());
            }

            if (message.Payload.Length > 140)
            {
                throw new ValidationException(ValidationMessages.MessageGreaterThan140Characters);
            }

            var result = Tweet(message);
            var jr = new Jobs.JobResult(message, true, String.Format("Tweet sent {0} {1} {2}", message.Settings.ScreenName, message.Payload, result.ToString()));
            base.Reply(jr);
        }

        private static Task<DirectMessage> Tweet(SendTweetJob message)
        {
            var cred = new LinqToTwitter.SingleUserInMemoryCredentialStore
            {
                ConsumerKey = message.Settings.ConsumerKey,
                ConsumerSecret = message.Settings.ConsumerSecret,
                AccessToken = message.Settings.AccessToken,
                AccessTokenSecret = message.Settings.AccessTokenSecret,
                OAuthToken = message.Settings.OAuthToken,
                OAuthTokenSecret = message.Settings.OAuthTokenSecret,
                ScreenName = message.Settings.ScreenName
            };
            var auth = new SingleUserAuthorizer();
            auth.CredentialStore = cred;
            var twitter = new TwitterContext(auth);
            var result = twitter.NewDirectMessageAsync(message.To,
                message.Payload + "  " + DateTime.Now.ToShortDateString());
            result.Wait();
            return result;
        }
    }
}