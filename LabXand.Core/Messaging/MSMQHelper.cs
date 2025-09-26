using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;

namespace LabXand.Core.Messaging.MSMQ
{
    public class MSMQHelper
    {
        public static void CreateQueue(string queuePath)
        {

            try
            {
                MessageQueue.Create(queuePath);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static bool ExistsQueue(string queuePath)
        {
            return MessageQueue.Exists(queuePath);
        }

        public static void SendMessage(string title, object attached, string queuePath)
        {
            try
            {
                if (title == null)
                    title = "[No Title]";
                // open the queue
                MessageQueue mq = new MessageQueue(queuePath);
                // set the message to durable.
                mq.DefaultPropertiesToSend.Recoverable = true;
                // set the formatter to Binary, default is XML
                mq.Formatter = new BinaryMessageFormatter();

                // send the command object
                mq.Send(attached, title);
                mq.Close();
                //BPMSDebug.Alert(string.Format("{0} Message as {1} sent.", title, DateTime.Now.ToShortDateString()));
            }
            catch (Exception e)
            {
                //BSI.Common.BPMSDebug.Alert(e.Message + " : Queue : "+queuePath , "MSMQHelper");
                throw e;

            }
        }

        public static void ReadMessage(string queuePath)
        {
            MessageQueue mq = new MessageQueue(queuePath);
            mq.Formatter = new BinaryMessageFormatter();
            mq.ReceiveCompleted += new ReceiveCompletedEventHandler(mq_ReceiveCompleted);
            mq.BeginReceive();
        }

        static void mq_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {

        }

    }
}
