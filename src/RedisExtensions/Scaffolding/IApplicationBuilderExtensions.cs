using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RedisExtensions.Abstractions;
using RedisExtensions.Attributes;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Reflection;

namespace RedisExtensions.Scaffolding
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseRedisProcessors(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices;
            var sub = services.GetService<ISubscriber>();

            var processors = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.GetCustomAttribute<ProcessorAttribute>() != null);
            
            foreach (var processor in processors)
            {
                var scope = services.CreateScope();
                var attribute = processor.GetCustomAttribute<ProcessorAttribute>();

                var ctor = processor.GetConstructors().First();
                var parameters = ctor.GetParameters()
                    .Select(param => scope.ServiceProvider.GetRequiredService(param.ParameterType));

                var instance = ctor.Invoke(parameters.ToArray());

                switch (instance)
                {
                    case IProcessor p:
                        sub.Subscribe(attribute.QueueName).OnMessage(p.OnMessage);
                        break;

                    case IAsyncProcessor ap:
                        sub.Subscribe(attribute.QueueName).OnMessage(ap.OnMessageAsync);
                        break;
                }
            }
        }
    }
}
