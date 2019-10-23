using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver
{
    internal class Config
    {
        public string HostName { get; set; }
        public string QueueName { get; set; }

        public Config()
        {
            HostName = Environment.GetEnvironmentVariable("hostname");
            QueueName = Environment.GetEnvironmentVariable("queuename");
        }
    }
}
