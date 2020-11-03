using StackExchange.Redis;
using System.Threading.Tasks;

namespace RedisExtensions.Abstractions
{
    public interface IAsyncProcessor
    {
        Task OnMessageAsync(ChannelMessage channelMessage);
    }
}
