using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Receiver.Contracts;
using Receiver.Logic;

namespace Receiver
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

           var listener = new Listener(new ConnectionFactory {HostName = config.HostName}, new FatherPrinter(), loggerFactory.CreateLogger("Listener"));
           listener.StartListening(config.QueueName);

           Console.ReadLine();
        }
    }
}
