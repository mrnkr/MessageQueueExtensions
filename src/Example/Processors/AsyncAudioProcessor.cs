using RedisExtensions.Abstractions;
using RedisExtensions.Attributes;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace RedisExtensions.Example.Processors
{
    [Processor("audio")]
    public sealed class AsyncAudioProcessor : IAsyncProcessor
    {
        public async Task OnMessageAsync(ChannelMessage channelMessage)
        {
            await Task.Delay(1000);
            Console.WriteLine($"Async >> {(string) channelMessage.Message}");
        }
    }
}
