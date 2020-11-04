using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MessageQueueExtensions.Abstractions;
using MessageQueueExtensions.Attributes;
using System.Linq;
using System.Reflection;

namespace MessageQueueExtensions.Scaffolding
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseProcessors(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices;
            var mq = services.GetRequiredService<IMessageQueue>();

            var processors = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.GetCustomAttribute<ProcessorAttribute>() != null);
            
            foreach (var processor in processors)
            {
                var attribute = processor.GetCustomAttribute<ProcessorAttribute>();
                var instance = ActivatorUtilities.CreateInstance(services, processor);

                switch (instance)
                {
                    case IProcessor p:
                        mq.RegisterProcessor(attribute.QueueName, p);
                        break;
                    case IAsyncProcessor ap:
                        mq.RegisterProcessor(attribute.QueueName, ap);
                        break;
                }
            }
        }
    }
}
