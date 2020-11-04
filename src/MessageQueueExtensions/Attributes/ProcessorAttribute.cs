using System;

namespace MessageQueueExtensions.Attributes
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
