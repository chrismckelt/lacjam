using Lacjam.Core;
using NServiceBus;

namespace Lacjam.Worker.Jobs
{
    public class SendTweetJob : JobBase, IMessage
    {
        public string To { get; set; }

        public Settings.TwitterSettings Settings { get; set; }
    }
}