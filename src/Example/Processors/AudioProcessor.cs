using RedisExtensions.Abstractions;
using RedisExtensions.Attributes;
using StackExchange.Redis;
using System;

namespace RedisExtensions.Example.Processors
{
    [Processor("audio")]
    public sealed class AudioProcessor : IProcessor
    {
        public void OnMessage(ChannelMessage channelMessage)
        {
            Console.WriteLine($"Non-async >> {(string) channelMessage.Message}");
        }
    }
}
