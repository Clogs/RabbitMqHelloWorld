using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using Receiver.Contracts;
using Receiver.Logic;
using Sender.Logic;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private const string TestingRabbitMqHostName = "localhost";
        private const string TestQueue = "testing";


        private Mock<ILogger> _mockLogger;
        private ConnectionFactory _connectionFactory;
        private Mock<IMessagePrinter> _mockPrinter;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger>();
            _connectionFactory = new ConnectionFactory() { HostName = TestingRabbitMqHostName };
            _mockPrinter = new Mock<IMessagePrinter>();
        }

        [Test]
        public void TestMessageSend()
        {
            var messenger = new Messenger(_connectionFactory, _mockLogger.Object );
            messenger.Send("Test message", TestQueue);
        }

        [Test]
        public void TestListenerInit()
        {
            var listener = new Listener(_connectionFactory, _mockPrinter.Object, _mockLogger.Object);
            listener.StartListening(TestQueue);
        }

        [Test]
        public void TestSendReceive()
        {
            Expression<Action<IMessagePrinter>> call = x => x.PrintMessage(It.IsAny<string>());
            _mockPrinter.Setup(call).Verifiable("Method not called");

            TestListenerInit();
            TestMessageSend();

            //IMessagePrinter.PrintMessage should be called once if a message was received 
            _mockPrinter.Verify(call,Times.Once);
        }

    }
}
