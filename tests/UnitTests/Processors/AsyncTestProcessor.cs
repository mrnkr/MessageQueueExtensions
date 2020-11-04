using MessageQueueExtensions.Abstractions;
using MessageQueueExtensions.Attributes;
using System.Threading.Tasks;

namespace MessageQueueExtensions.UnitTests.Processors
{
    [Processor("queue")]
    internal sealed class AsyncTestProcessor : IAsyncProcessor
    {
        public Task OnMessageAsync(object message)
        {
            throw new System.NotImplementedException();
        }
    }
}
