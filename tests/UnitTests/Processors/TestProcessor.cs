using MessageQueueExtensions.Abstractions;
using MessageQueueExtensions.Attributes;

namespace MessageQueueExtensions.UnitTests.Processors
{
    [Processor("queue")]
    internal sealed class TestProcessor : IProcessor
    {
        public void OnMessage(object message)
        {
            throw new System.NotImplementedException();
        }
    }
}
