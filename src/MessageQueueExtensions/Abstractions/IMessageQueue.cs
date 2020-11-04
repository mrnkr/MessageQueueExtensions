namespace MessageQueueExtensions.Abstractions
{
    public interface IMessageQueue
    {
        void RegisterProcessor(string queueName, IProcessor processor);
        void RegisterProcessor(string queueName, IAsyncProcessor processor);
    }
}
