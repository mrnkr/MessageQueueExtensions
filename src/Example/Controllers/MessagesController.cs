using Microsoft.AspNetCore.Mvc;
using MessageQueueExtensions.Example.Dtos;
using StackExchange.Redis;

namespace MessageQueueExtensions.Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class MessagesController : ControllerBase
    {
        private ISubscriber Subscriber { get; set; }

        public MessagesController(ISubscriber subscriber)
        {
            Subscriber = subscriber;
        }

        [HttpPost]
        public ActionResult<string> PostMessage([FromBody] MessageDto message)
        {
            Subscriber.Publish("audio", message.Body);
            return "Ok";
        }
    }
}
