using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var producer = new Producers.Producer(configuration);
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    await producer.ProduceAsync("Events", $"event message {new Random().Next(1000)}");
                }
                else
                {
                    break;
                }
            }
        }
    }
}
