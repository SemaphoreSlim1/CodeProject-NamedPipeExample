using Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QueueWriter.Web.Controllers
{
    public class QueueController : ApiController
    {
        public HttpResponseMessage Post()
        {
            using (var mq = new MessageQueue(ConfigurationManager.AppSettings["OperationMessageQueuePath"]))
            {
                var qItem = new QueuedWorkItem() { Name = "Web Queue Item" };
                using (var msg = new System.Messaging.Message(qItem))
                {
                    msg.Label = "Queued Item from the web";
                    msg.Formatter = new XmlMessageFormatter(new Type[] { typeof(QueuedWorkItem) });
                    mq.Send(msg);
                    System.Console.WriteLine("Message sent. Message ID is {0}", msg.Id);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
