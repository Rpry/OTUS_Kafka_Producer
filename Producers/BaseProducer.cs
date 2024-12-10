using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Producer.Models;
using Producer.Settings;
using Producer.Utils;

namespace Producer.Producers;

public class BaseProducer<TKey, TValue> where TValue : IKafkaMessage
{
    protected readonly ILogger Logger;
    
    public IProducer<TKey, TValue> Producer { get; }
    
    
    public BaseProducer(KafkaOptions kafkaOptions, ILogger logger)
    {
        Logger = logger;
        var producerConfig = new ProducerConfig()
        {
            BootstrapServers = kafkaOptions.BootstrapServers,
            //Partitioner = Partitioner.Random,
            Partitioner = Partitioner.Consistent,

        };
        var producerBuilder = new ProducerBuilder<TKey, TValue>(producerConfig);
        Producer = producerBuilder
            .SetErrorHandler((_, error) => Logger.LogError(error.Reason))
            .SetLogHandler((_, message) =>  Logger.LogInformation(message.Message))
            .SetKeySerializer((ISerializer<TKey>)Serializers.Utf8)
            //.SetKeySerializer((ISerializer<TKey>)Serializers.Int64)
            .SetValueSerializer(new KafkaMessageSerializer<TValue>())
            .Build();
    }
}