using System;
using System.Messaging;


namespace MSMQ
{
    public class SimpleRouter
    {
        private readonly MessageQueue outQueue1;
        private readonly MessageQueue outQueue2;
        private MessageQueue inQueue;

        private bool toggle = false;


        public SimpleRouter(MessageQueue inQueue,
            MessageQueue outQueue1, MessageQueue outQueue2)
        {
            this.inQueue = inQueue;
            this.outQueue1 = outQueue1;
            this.outQueue2 = outQueue2;

            inQueue.ReceiveCompleted += OnMessage;
            inQueue.BeginReceive();
        }


        private bool IsConditionFulfilled()
        {
            toggle = !toggle;
            return toggle;
        }


        private void OnMessage(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            var mq = (MessageQueue)source;
            Message message = mq.EndReceive(asyncResult.AsyncResult);

            if (IsConditionFulfilled())
                outQueue1.Send(message);
            else
                outQueue2.Send(message);
            mq.BeginReceive();
        }
    }
}
