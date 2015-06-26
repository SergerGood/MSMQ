using System;
using System.Messaging;


namespace MSMQ
{
    public sealed class Processor
    {
        private readonly MessageQueue inputQueue;
        private readonly MessageQueue outputQueue;


        public Processor(MessageQueue inputQueue, MessageQueue outputQueue)
        {
            this.inputQueue = inputQueue;
            this.outputQueue = outputQueue;
        }


        public void Process()
        {
            inputQueue.ReceiveCompleted += OnReceiveCompleted;
            inputQueue.BeginReceive();
        }


        private Message ProcessMessage(Message m)
        {
            Console.WriteLine("Received Message: " + m.Body);
            return (m);
        }


        private void OnReceiveCompleted(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            var mq = (MessageQueue)source;

            Message inputMessage = mq.EndReceive(asyncResult.AsyncResult);

            inputMessage.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });

            Message outputMessage = ProcessMessage(inputMessage);
            outputQueue.Send(outputMessage);

            mq.BeginReceive();
        }
    }
}
