﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SuperBike.Consumer.DataAccess;
using SuperBike.Consumer.Entities;
using SuperBike.Domain.Events;
using System.Text;

namespace SuperBike.Consumer.ServiceHandler
{
    public class ConsumerMessageBrocker : BackgroundService
    {
        ILogger<ConsumerMessageBrocker> _logger;
        DataAccessEvent _dataAccess;
        public ConsumerMessageBrocker(ILogger<ConsumerMessageBrocker> logger, DataAccessEvent dataAccess)
        {
            _logger = logger;
            _dataAccess = dataAccess;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
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
        }

        public override void Dispose()
        {
            _dataAccess.Dispose();
            base.Dispose();
        }
    }
}
