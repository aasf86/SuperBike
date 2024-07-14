using RabbitMQ.Client;
using SuperBike.Domain.Contracts.Services;
using System.Text;
using System.Text.Json;

namespace SuperBike.Infrastructure.Services
{
    public class MessageBrokerRabbitMq : IMessageBroker
    {
        private readonly ConnectionFactory _factory;
        public MessageBrokerRabbitMq(string host, int port, string userName, string password) => _factory = new ConnectionFactory 
        { 
            HostName = host,
            //Port = port,
            //UserName = password,
            //Password = userName
        };

        public async Task Publish<T>(T @event, string queue)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            var eventSerialise = JsonSerializer.Serialize(@event);

            channel.QueueDeclare(
                queue: queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            var body = Encoding.UTF8.GetBytes(eventSerialise);

            channel.BasicPublish(
                exchange: "",
                routingKey: queue,
                basicProperties: null,
                body: body);
        }
    }
}
