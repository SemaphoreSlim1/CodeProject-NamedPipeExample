using NamedPipe.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipe
{
    public class Receiver : IDisposable
    {
        public const String DefaultPipeName = "Pipe1";

        private PipeService _ps = new PipeService();
        private ServiceHost _host = null;        
        private Boolean _operational { get; set; }

        #region PipeName

        private String _PipeName = String.Empty;

        /// <summary>
        /// Gets the name of the pipe being used by this reciever
        /// </summary>
        public String PipeName
        {
            get { return _PipeName; }            
        }

        #endregion

        public Receiver(Action<String> messageReceivedAction) : this(DefaultPipeName, messageReceivedAction) { }

        public Receiver(String pipeName, Action<String> messageReceivedAction)
        {
            _PipeName = pipeName;
            _ps.MessageReceived = messageReceivedAction;
        }        


        /// <summary>
        /// Stops the hosting service
        /// </summary>
        public void ServiceOff()
        {
            if (_host == null)
            { return; } //already turned off

            if (_host.State != CommunicationState.Closed)
            { _host.Close(); }

            _operational = false;
        }

        /// <summary>
        /// Performs the act of starting the WCF host service
        /// </summary>
        /// <returns>true, upon success</returns>
        public Boolean ServiceOn()
        {

            try
            {
                _host = new ServiceHost(_ps, new Uri(PipeService.URI));
                _host.AddServiceEndpoint(typeof(IPipeService), new NetNamedPipeBinding(), _PipeName);
                _host.Open();
                _operational = true;
            }
            catch (Exception ex)
            {
                _operational = false;
            }

            return _operational;
        }

        #region IDisposable
        //Read http://stackoverflow.com/a/538238/85297 for best practices regarding implementation of IDisposable

        ~Receiver()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(Boolean safeToDisposeManagedObjects)
        {
            if (safeToDisposeManagedObjects == false)
            { return; } //we have no unmaaged objects to worry about for purposes of this class

            _ps = null;
        }

        #endregion

    }
}
