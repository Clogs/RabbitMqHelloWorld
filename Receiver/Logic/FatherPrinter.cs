using System;
using Receiver.Contracts;

namespace Receiver.Logic
{
    public class FatherPrinter : IMessagePrinter
    {
        public void PrintMessage(string message)
        {
            Console.WriteLine($"Hello {message}, I am your father!");
        }
    }
}
