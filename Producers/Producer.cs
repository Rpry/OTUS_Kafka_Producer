using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Producer.Producers
{
    public class Producer
    {
        public static async Task Run(ProducerConfig config)
        {
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Enter)
                    {
                        await producer.ProduceAsync("Events",
                            new Message<string, string>
                            {
                                Key = Guid.NewGuid().ToString(),
                                Value = "event message"
                            });
                        Console.WriteLine("A message was sent");
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}