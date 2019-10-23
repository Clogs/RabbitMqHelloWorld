using System;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Sender.Logic
{
    public class Messenger 
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;


        public Messenger(IConnectionFactory connectionFactory, ILogger logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }


        public void Send(string message,string queueName)
        {
            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);

                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName,exclusive:false);
                    channel.BasicPublish(exchange: "",
                        routingKey: queueName,
                        body: messageBytes);

                    _logger.LogDebug($"Sent {message} on {queueName}");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error in Send");
                throw;
            }
        }

    }
}
