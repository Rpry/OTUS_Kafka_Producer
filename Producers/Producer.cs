using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Producer.Settings;

namespace Producer.Producers
{
    public class Producer
    {
        private ProducerConfig _config;
        
        public Producer(IConfiguration config)
        {
            _config = GetKafkaProducerConfig(config);
        }
        
        public async Task ProduceAsync(string topicName, string message)
        {
            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                await producer.ProduceAsync(topicName,
                    new Message<string, string>
                    {
                        Key = Guid.NewGuid().ToString(),
                        Value = message
                    });

                Console.WriteLine(message);
            }
        }
        
        private ProducerConfig GetKafkaProducerConfig(IConfiguration configuration)
        {
            var kafkaSettings = configuration.Get<ApplicationSettings>().KafkaSettings;
            var config = new ProducerConfig()
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
            };
            return config;
        }
    }
}