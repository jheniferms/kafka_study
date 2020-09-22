using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProducerStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProducerStore.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Post()
        {
            var order = new Order
            {
                Id = 1,
                Value = 2
            };
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    //var result = await producer.ProduceAsync("new-pet", new Message<Null, string> { Value = JsonSerializer.Serialize(pet) });

                    for (int i = 1; i < 4; i++)
                    {
                        var result = await producer.ProduceAsync("LOJA_NOVO_PEDIDO", new Message<Null, string> { Value = i.ToString() });

                        Console.WriteLine($"Mensagem: {i.ToString()} | " + $"Status: { result.Status.ToString()}");
                    }
                    return Ok();
                    //return Ok(result);
                }
                catch (ProduceException<Null, string> ex)
                {
                    return StatusCode(500, ex);
                }
            }
        }
    }
}
