using System;
using System.Messaging;


namespace MSMQ
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MessageQueue input = CreatePublicQueues(".\\inputMsg");
            MessageQueue output = CreatePublicQueues(".\\outputMsg");

            //

            while (true)
            {
                input.Send("123");

                Message journalMessage = input.Receive();
            }
        }


        public static MessageQueue CreatePublicQueues(string path)
        {
            if (!MessageQueue.Exists(path))
            {
                return MessageQueue.Create(path);
            }

            return new MessageQueue(path);
        }


        private static void CreateProcessor(MessageQueue input, MessageQueue output)
        {
            var processor = new Processor(input, output);
            processor.Process();
        }


        private static void CreateSimpleRouter(MessageQueue input, MessageQueue output)
        {
            var router = new SimpleRouter(input, output, output);
        }
    }
}
