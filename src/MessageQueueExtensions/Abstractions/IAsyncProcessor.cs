using System.Threading.Tasks;

namespace MessageQueueExtensions.Abstractions
{
    public interface IAsyncProcessor
    {
        Task OnMessageAsync(object message);
    }
}
