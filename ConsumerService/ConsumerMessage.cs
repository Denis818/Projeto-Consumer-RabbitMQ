using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Options;
using Integradora.Consumer.RabbitMQ.Interfaces;
using RabbitMQ.Client.Exceptions;
using Integradora.Consumer.RabbitMQ.Models.AppSettings;

namespace Integradora.Consumer.RabbitMQ.ConsumerService
{
    public class ConsumerMessage : IConsumerMessage
    {
        public RabbitMqConfiguration RabbitMqConfiguration { get; }

        public ConsumerMessage(IOptions<RabbitMqConfiguration> rabbitMqConfiguration)
        {
            RabbitMqConfiguration = rabbitMqConfiguration.Value;
        }
        public void ConsumerMessageRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = RabbitMqConfiguration.HostName,

                UserName = RabbitMqConfiguration.UserName,

                Password = RabbitMqConfiguration.Password,

                VirtualHost = RabbitMqConfiguration.VirtualHost,

                DispatchConsumersAsync = true
            };

            try
            {
                using var connection = factory.CreateConnection();

                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: RabbitMqConfiguration.QueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.Received += async (model, entrega) =>
                {
                    var body = entrega.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    await Task.Yield();

                    Console.WriteLine("[x] Recebido com sucesso {0} \n---------", message);
                };

                channel.BasicConsume(queue: RabbitMqConfiguration.QueueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
            catch (BrokerUnreachableException)
            {
                Console.WriteLine("[*] Error: Sem Conexão");
            }
        }
    }
}
