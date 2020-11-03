using System;

namespace RedisExtensions.Attributes
{
    public class ProcessorAttribute : Attribute
    {
        public string QueueName { get; }

        public ProcessorAttribute(string queueName)
        {
            QueueName = queueName;
        }
    }
}
