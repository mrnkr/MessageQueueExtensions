using MessageQueueExtensions.Attributes;

namespace MessageQueueExtensions.UnitTests.Processors
{
    [Processor("queue")]
    internal sealed class InvalidProcessor
    {
        public void OnMessage(object message)
        {
            throw new System.NotImplementedException();
        }
    }
}
