using MessageQueueExtensions.Abstractions;
using MessageQueueExtensions.Attributes;
using System;

namespace MessageQueueExtensions.Example.Processors
{
    [Processor("audio")]
    public sealed class AudioProcessor : IProcessor
    {
        public void OnMessage(object message)
        {
            Console.WriteLine($"Non-async >> {message}");
        }
    }
}
