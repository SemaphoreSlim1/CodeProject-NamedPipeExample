using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipe.Communication
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal class PipeService : IPipeService
    {
        public static string URI = "net.pipe://localhost/Pipe";

        public Action<String> MessageReceived = null;

        public void RecieveMessage(String message)
        {
            if (MessageReceived != null)
            { MessageReceived(message); }
        }
    }
}
