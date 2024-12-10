using System.IO;
using System.Text.Json;
using Confluent.Kafka;
using Producer.Models;

namespace Producer.Utils;

public class KafkaMessageSerializer<T> : ISerializer<T> where T: IKafkaMessage
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        using (var ms = new MemoryStream())
        {
            string jsonString = JsonSerializer.Serialize(data);
            var writer = new StreamWriter(ms);

            writer.Write(jsonString);
            writer.Flush();
            ms.Position = 0;

            return ms.ToArray();
        }
    }
}