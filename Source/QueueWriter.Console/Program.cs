using Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace QueueWriter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GatherAndSend();
        }

        private static void GatherAndSend()
        {
            System.Console.WriteLine("type some stuff to queue up");
            var input = System.Console.ReadLine();


            using (var mq = new MessageQueue(ConfigurationManager.AppSettings["OperationMessageQueuePath"]))
            {
                var qItem = new QueuedWorkItem() { Name = input };
                using (var msg = new System.Messaging.Message(qItem))
                {
                    msg.Label = "Queued Item from the console";
                    msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(QueuedWorkItem) });
                    mq.Send(msg);
                    System.Console.WriteLine("Message sent. Message ID is {0}", msg.Id);
                }
            }

            System.Console.WriteLine("write another message to the queue? Y/N");
            var decision = System.Console.ReadLine();

            if (decision == "y")
            { GatherAndSend(); }

        }
    }
}
