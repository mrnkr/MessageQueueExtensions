# RedisExtensions

[![license](https://img.shields.io/github/license/mrnkr/RedisExtensions)](https://github.com/mrnkr/RedisExtensions/blob/master/LICENSE)

The boilerplate you always want but never take the time to write for when you want your API to also have some Redis stuff going on in the background.

Now, seriously, if you are thinking of using `StackExchange.Redis` as a means of inter-service communication and come across the question: "But where should I put the code that processes new messages?" then this library intends to give you a simple and familiar solution to that.

This is heavily inspired by [NestJS Queue handling](https://docs.nestjs.com/techniques/queues).

## Getting Started

First you need to register your redis subscriber as a service and tell your app builder to instantiate all the processors you have for your MQs.

```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ISubscriber>(sp =>
        {
            var redis = ConnectionMultiplexer.Connect(Configuration.GetSection("Redis:Host").Value);
            return redis.GetSubscriber();
        });

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

The `IApplicationBuilder.UseRedisProcessors` extension method that I added is the one that instantiates all the processors you declare in your assembly and inject their dependencies. This way you have the same dependency injection capabilities you do with your ASP.NET Core controllers and services in general.

**IMPORTANT** Processors cannot have more than one constructor declared. This is because the `IApplicationBuilder.UseRedisProcessors` method takes the first constructor to instantiate the class with its dependencies. Note that if you don't need to declare a constructor **this will still work** thanks to the default, implicit, patameterless constructor being available. **The problem is when you declare two explicit constructors**.

As an example, say you had an MQ named `test` and you wanted to print every value (of type `string`) that went into that MQ. This is what that processor would look like:

```cs
[Processor("test")]
public sealed class TestProcessor : IProcessor
{
    public void OnMessage(ChannelMessage channelMessage)
    {
        Console.WriteLine((string) channelMessage.Message);
    }
}
```

If, instead, you need to do some async operation each time your MQ has a message pushed to it then the changes are subtle, here is a simple example that does the same thing the synchronous processor does but waits a second before doing it:

```cs
[Processor("test")]
public sealed class AsyncTestProcessor : IAsyncProcessor
{
    public Task OnMessageAsync(ChannelMessage channelMessage)
    {
        await Task.Delay(1000);
        Console.WriteLine((string) channelMessage.Message);
    }
}
```
