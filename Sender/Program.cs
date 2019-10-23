using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Sender.Logic;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config();
            var loggerFactory = LoggerFactory.Create(builder => {
                    builder.AddConsole();
                }
            );

            Console.WriteLine("Name");
            var name = "HAI";

            var messenger = new Messenger(new ConnectionFactory { HostName = config.HostName }, loggerFactory.CreateLogger("Messenger"));

            messenger.Send(name, config.QueueName);

            Console.ReadLine();
        }

        
    }
}
