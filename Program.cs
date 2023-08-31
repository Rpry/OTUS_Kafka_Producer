using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Producer.Settings;

namespace Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            await Producers.Producer.Run(GetKafkaProducerConfig(configuration));
        }

        private static ProducerConfig GetKafkaProducerConfig(IConfiguration configuration)
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
