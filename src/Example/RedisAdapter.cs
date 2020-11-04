using MessageQueueExtensions.Abstractions;
using StackExchange.Redis;
using System;

namespace MessageQueueExtensions.Example
{
    public class RedisAdapter : IMessageQueue
    {
        private ISubscriber Subscriber { get; }

        public RedisAdapter(ISubscriber subscriber)
        {
            Subscriber = subscriber;
        }

        public void RegisterProcessor(string queueName, IProcessor processor) =>
            Subscriber.Subscribe(queueName)
                .OnMessage(msg => processor.OnMessage(msg.Message));

        public void RegisterProcessor(string queueName, IAsyncProcessor processor) =>
            Subscriber.Subscribe(queueName)
                .OnMessage(msg => processor.OnMessageAsync(msg.Message));
    }
}
