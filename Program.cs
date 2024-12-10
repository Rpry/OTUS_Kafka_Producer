using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Producer.Models;
using Producer.Options;

namespace Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var options = configuration.Get<ApplicationOptions>();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            
            var producer = new Producers.OrderCreatedMessageProducer(options, logger);
            
            /*
            long orderId = new Random().Next();
            await producer.ProduceAsync(orderId, new OrderCreatedMessage
                {
                    ChangedAt = DateTimeOffset.UtcNow,
                    Id = orderId,
                    State = OrderState.Created,
                },
                CancellationToken.None);
            */
            
            /*
            long orderId = new Random().Next();
            await producer.ProduceAsync(orderId, 2, new OrderCreatedMessage
                {
                    ChangedAt = DateTimeOffset.UtcNow,
                    Id = orderId,
                    State = OrderState.Created,
                },
                CancellationToken.None);
*/
            #region Cycle
            
            for (int i = 0; i < 10; i++)
            {
                //long orderId = new Random().Next();
                long orderId = i;
                await producer.ProduceAsync(orderId.ToString(), new OrderCreatedMessage
                //await producer.ProduceAsync(orderId, new OrderCreatedMessage
                    {
                        ChangedAt = DateTimeOffset.UtcNow,
                        Id = orderId,
                        State = OrderState.Created
                    },
                    CancellationToken.None);
            }

            #endregion
        }
    }
}
