using Confluent.Kafka;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerOrder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                GroupId = "new-pet-group-3",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("LOJA_NOVO_PEDIDO");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = consumer.Consume(cts.Token);

                        Console.WriteLine("Got Pet {0}", cr.Message.Value);
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine("Error {0}", ex.Message);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}