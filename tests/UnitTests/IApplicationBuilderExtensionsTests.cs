using Moq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MessageQueueExtensions.Abstractions;
using MessageQueueExtensions.Scaffolding;
using MessageQueueExtensions.UnitTests.Processors;
using System;
using Xunit;

namespace MessageQueueExtensions.UnitTests
{
    public class IApplicationBuilderExtensionsTests
    {
        [Fact]
        public void ShouldRegisterProcessor()
        {
            var services = new ServiceCollection();

            var mqMock = new Mock<IMessageQueue>();
            services.AddSingleton<IMessageQueue>(sp => mqMock.Object);

            var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
            applicationBuilder.UseProcessors();

            mqMock.Verify(mq => mq.RegisterProcessor("queue", It.IsAny<TestProcessor>()), Times.Once);
        }

        [Fact]
        public void ShouldRegisterAsyncProcessor()
        {
            var services = new ServiceCollection();

            var mqMock = new Mock<IMessageQueue>();
            services.AddSingleton<IMessageQueue>(sp => mqMock.Object);

            var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
            applicationBuilder.UseProcessors();

            mqMock.Verify(mq => mq.RegisterProcessor("queue", It.IsAny<AsyncTestProcessor>()), Times.Once);
        }

        [Fact]
        public void ShouldThrowWhenNoIRedisIsRegistered()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var services = new ServiceCollection();
                var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
                applicationBuilder.UseProcessors();
            });
        }
    }
}
