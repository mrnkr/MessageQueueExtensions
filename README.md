# MessageQueueExtensions

[![NuGet version][nuget-image]][nuget-url]
[![Downloads][downloads-image]][nuget-url]
[![Build Status](https://travis-ci.com/mrnkr/MessageQueueExtensions.svg?branch=master)](https://travis-ci.com/mrnkr/MessageQueueExtensions)
[![codecov](https://codecov.io/gh/mrnkr/MessageQueueExtensions/branch/master/graph/badge.svg)](https://codecov.io/gh/mrnkr/MessageQueueExtensions)
[![license][license]](https://github.com/mrnkr/MessageQueueExtensions/blob/master/LICENSE)

[nuget-image]:https://img.shields.io/nuget/v/MessageQueueExtensions
[nuget-url]:https://www.nuget.org/packages/MessageQueueExtensions
[downloads-image]:https://img.shields.io/nuget/dt/MessageQueueExtensions
[license]:https://img.shields.io/github/license/mrnkr/MessageQueueExtensions

The boilerplate you always want but never take the time to write for when you want your API to also have some Redis stuff going on in the background.

Now, seriously, if you are thinking of using `StackExchange.Redis` as a means of inter-service communication and come across the question: "But where should I put the code that processes new messages?" then this library intends to give you a simple and familiar solution to that.

This is heavily inspired by [NestJS Queue handling](https://docs.nestjs.com/techniques/queues).

## Getting Started

First you need to register a class implementing `IMessageQueue` as a service and tell your app builder to instantiate all the processors you have for your MQs.

**This library is independent of the underlying message queue** so even if I'm using redis in this example you can use any message queue you like.

```cs
public class RedisAdapter : IMessageQueue
{
    private ISubscriber Subscriber { get; }

    public RedisAdapter(IConfiguration config)
    {
        var redis = ConnectionMultiplexer.Connect(config.GetSection("Redis:Host").Value);
        Subscriber = redis.GetSubscriber();
    }

    public void RegisterProcessor(string queueName, IProcessor processor) =>
        Subscriber.Subscribe(queueName)
            .OnMessage(msg => processor.OnMessage(msg.Message));

    public void RegisterProcessor(string queueName, IAsyncProcessor processor) =>
        Subscriber.Subscribe(queueName)
            .OnMessage(msg => processor.OnMessageAsync(msg.Message));
}
```

```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMessageQueue, RedisWrapper>();
        // ...
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRedisProcessors();
        // ...
    }
}
```

I said processors, right? Processors are classes that implement either `IProcessor` or `IAsyncProcessor` and their responsibility is to handle new messages that get published to an MQ. The way they know what MQ they need to listen to is via the `ProcessorAttribute`.

The `IApplicationBuilder.UseProcessors` extension method that I added is the one that instantiates all the processors you declare in your assembly and inject their dependencies. This way you have the same dependency injection capabilities you do with your ASP.NET Core controllers and services in general.

As an example, say you had an MQ named `test` and you wanted to print every value (of type `string`) that went into that MQ. This is what that processor would look like:

```cs
[Processor("queue")]
public sealed class TestProcessor : IProcessor
{
    public void OnMessage(object message)
    {
        Console.WriteLine(message);
    }
}
```

If, instead, you need to do some async operation each time your MQ has a message pushed to it then the changes are subtle, here is a simple example that does the same thing the synchronous processor does but waits a second before doing it:

```cs
[Processor("test")]
public sealed class AsyncTestProcessor : IAsyncProcessor
{
    public Task OnMessageAsync(object message)
    {
        await Task.Delay(1000);
        Console.WriteLine(message);
    }
}
```
