using StackExchange.Redis;

namespace RedisExtensions.Abstractions
{
    public interface IProcessor
    {
        void OnMessage(ChannelMessage channelMessage);
    }
}
