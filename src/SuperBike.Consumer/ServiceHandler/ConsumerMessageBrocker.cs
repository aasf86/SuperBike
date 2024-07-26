using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SuperBike.Consumer.DataAccess;
using SuperBike.Consumer.Entities;
using SuperBike.Domain.Events;
using System.Text;

namespace SuperBike.Consumer.ServiceHandler
{
    public class ConsumerMessageBrocker : BackgroundService
    {
        private readonly ILogger<ConsumerMessageBrocker> _logger;
        private readonly DataAccessEvent _dataAccess;        
        private readonly ConnectionFactory _factory;
        public ConsumerMessageBrocker(ILogger<ConsumerMessageBrocker> logger, DataAccessEvent dataAccess, IConfigurationManager config)
        {            
            _logger = logger;            
            _dataAccess = dataAccess;            
            _factory = new ConnectionFactory { HostName = config.GetSection("RabbitMqHost:Host").Value };            
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            using var connection = await Task.Run(async () =>
            {
                IConnection connectionRabbit = null;
                TRY_AGAIN:

                try
                {
                    connectionRabbit = _factory.CreateConnection();
                }
                catch
                {
                    _logger.LogWarning("===>>>Conexão com rabbit falhou!!<<<===");
                    await Task.Delay(1000);
                    goto TRY_AGAIN;
                }

                return connectionRabbit;
            });
            using var channel = connection.CreateModel();

            _logger.LogInformation("Iniciando consume de msg");

            channel.QueueDeclare(
                queue: Queues.Motorcycle.MOTORCYCLE_INSERTED,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, args) =>
            {
                try
                {
                    var body = args.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    var motorcycleInsertedEvent = System.Text.Json.JsonSerializer.Deserialize<MotorcycleInsertedEvent>(message);
                    _logger.LogInformation("Evento recebido {RequestId}", motorcycleInsertedEvent.RequestId);
                    channel.BasicAck(args.DeliveryTag, false);

                    if (motorcycleInsertedEvent.Year == 2024)
                    {
                        motorcycleInsertedEvent.MQ = "RabbitMQ";
                        motorcycleInsertedEvent.Queue = Queues.Motorcycle.MOTORCYCLE_INSERTED;
                        motorcycleInsertedEvent.MotorcycleId = motorcycleInsertedEvent.Id;
                        motorcycleInsertedEvent.Id = 0;
                        await _dataAccess.Insert(motorcycleInsertedEvent);
                    }
                    body = null;
                }
                catch (Exception exc)
                {
                    channel.BasicNack(args.DeliveryTag, false, true);
                    _logger.LogError(exc, exc.Message);
                }
            };

            channel.BasicConsume(
                queue: Queues.Motorcycle.MOTORCYCLE_INSERTED,
                autoAck: false,
                consumer: consumer);

            while (!stoppingToken.IsCancellationRequested) { await Task.Delay(1000); }
        }

        public override void Dispose()
        {
            _dataAccess.Dispose();
            base.Dispose();
        }
    }
}
