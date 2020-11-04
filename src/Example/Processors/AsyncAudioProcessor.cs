using MessageQueueExtensions.Abstractions;
using MessageQueueExtensions.Attributes;
using System;
using System.Threading.Tasks;

namespace MessageQueueExtensions.Example.Processors
{
    [Processor("audio")]
    public sealed class AsyncAudioProcessor : IAsyncProcessor
    {
        public async Task OnMessageAsync(object message)
        {
            await Task.Delay(1000);
            Console.WriteLine($"Async >> {message}");
        }
    }
}
