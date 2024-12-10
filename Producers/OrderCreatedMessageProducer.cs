using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Producer.Models;
using Producer.Options;

namespace Producer.Producers
{
    public class OrderCreatedMessageProducer: BaseProducer<string, OrderCreatedMessage>
    {
        private const string TopicName = "order_events";
        //private const string TopicName = "topicname2";
        
        public OrderCreatedMessageProducer(
            ApplicationOptions applicationOptions,
            ILogger logger):
            base(applicationOptions.KafkaOptions, logger)
        {
        }
        
        public async Task ProduceAsync(string key, OrderCreatedMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await Producer.ProduceAsync(TopicName,  new Message<string, OrderCreatedMessage>
                    {
                        Key = key,
                        Value = message
                    },
                    cancellationToken);
                var loggedKey = key != null ? key : "null";
                Logger.LogInformation($"Message for order with id {loggedKey} sent");
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
        }
        
        public async Task ProduceAsync(string orderId, int partitionId, OrderCreatedMessage message, CancellationToken cancellationToken)
        {
            var partition = new TopicPartition(TopicName, new Partition(partitionId));
            await Producer.ProduceAsync(partition,  new Message<string, OrderCreatedMessage>
                {
                    Key = orderId,
                    Value = message
                },
                cancellationToken);
            Logger.LogInformation($"Message for order with id {orderId} sent");
        }
    }
}