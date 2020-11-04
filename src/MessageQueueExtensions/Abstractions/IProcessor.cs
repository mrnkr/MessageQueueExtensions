namespace MessageQueueExtensions.Abstractions
{
    public interface IProcessor
    {
        void OnMessage(object message);
    }
}
