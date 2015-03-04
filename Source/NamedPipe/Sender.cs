using NamedPipe.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipe
{
    public class Sender
    {
        private static Object _Lock = new Object();
        private static EndpointAddress _endpointAddress = new EndpointAddress(String.Format("{0}/{1}", PipeService.URI, Receiver.DefaultPipeName));        

        /// <summary>
        /// Attempts to send the message to the proxy at the pre-configured endpoint
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns>True, upon success</returns>
        public static Boolean SendMessage(String message)
        {
            var success = false;
            try
            {
                lock (_Lock) //ensure thread exclusivity when sending messages across the wire
                {
                    var proxy = ChannelFactory<IPipeService>.CreateChannel(new NetNamedPipeBinding(), _endpointAddress);                    
                    proxy.RecieveMessage(message);                     
                }

                success = true;
            }
            catch (Exception ex) //Most likely, there was nobody to send a message to.
            { } //TODO : Add some logging

            return success;
        }       
    }
}
