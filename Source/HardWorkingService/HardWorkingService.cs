using Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HardWorkingService
{
    public partial class HardWorkingService : ServiceBase
    {
        #region OperationMessageQueue

        private MessageQueue _OperationMessageQueue;

        protected MessageQueue OperationMessageQueue
        {
            get
            {
                if (_OperationMessageQueue == null)
                {
                    _OperationMessageQueue = new MessageQueue(ConfigurationManager.AppSettings["OperationMessageQueuePath"]);
                    _OperationMessageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(QueuedWorkItem) });
                    _OperationMessageQueue.ReceiveCompleted += Mq_ReceiveCompleted;
                }
                return _OperationMessageQueue;
            }
        }        

        #endregion

        private void Mq_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            using (var msg = OperationMessageQueue.EndReceive(e.AsyncResult))
            {
                try
                {
                    var qItem = msg.Body as QueuedWorkItem;
                    ProcessQueuedWorkItem(qItem);
                }
                catch (Exception ex)
                {
                    //TODO : Write to the log we failed in some way
                    Environment.Exit(Environment.ExitCode);
                }
            }

            OperationMessageQueue.BeginReceive();
        }

        private void ProcessQueuedWorkItem(QueuedWorkItem item)
        {
            //ok, so obviously here is where you're going to do your long-running logic.
            //for purposes of this demo, we're just going to do some bogus work
            NamedPipe.Sender.SendMessage(String.Format("Starting work on {0}", item.Name));

            var delay = new TimeSpan(0,0,3);
            for(var i = 0; i < 5; i++)
            {
                NamedPipe.Sender.SendMessage(String.Format("beginning work on item {0} of 5 for {1}", i, item.Name));
                System.Threading.Thread.Sleep(delay);
                NamedPipe.Sender.SendMessage(String.Format("completed work on item {0} of 5 for {1}", i, item.Name));
            }

            NamedPipe.Sender.SendMessage(String.Format("Completed work on {0}", item.Name));
        }

        public HardWorkingService()
        {
            InitializeComponent();            
        }

        protected override void OnStart(string[] args)
        {
            OperationMessageQueue.BeginReceive();
        }

        protected override void OnStop()
        {
            OperationMessageQueue.ReceiveCompleted -= Mq_ReceiveCompleted;
            OperationMessageQueue.Dispose();
        }

        protected override void OnPause()
        {
            OperationMessageQueue.ReceiveCompleted -= Mq_ReceiveCompleted;
        }

        protected override void OnContinue()
        {
            OperationMessageQueue.ReceiveCompleted += Mq_ReceiveCompleted;
            OperationMessageQueue.BeginReceive();
        }
    }
}
