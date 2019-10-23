using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Receiver.Contracts;
using Microsoft.Extensions.Logging;

namespace Receiver.Logic
{
    public class Listener
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IMessagePrinter _messagePrinter;
        private readonly ILogger _logger;

        public Listener(IConnectionFactory connectionFactory,IMessagePrinter messagePrinter, ILogger logger)
        {
            _connectionFactory = connectionFactory;
            _messagePrinter = messagePrinter;
            _logger = logger;
        }

        public void StartListening(string queueName)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, exclusive: false);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += MessageReceived;
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                    _logger.LogDebug($"Started listening on {queueName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in StartListening");
                throw;
            }
            
        }

        public void MessageReceived(object model, BasicDeliverEventArgs eventArgs)
        {
            try
            {
                var messageBytes = eventArgs.Body;
                var message = Encoding.UTF8.GetString(messageBytes);

                _logger.LogDebug($"Received: \"{message}\" on channel: {eventArgs.RoutingKey}");

                _messagePrinter.PrintMessage(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in MessageReceived");
                throw;
            }
        }

    }
}
