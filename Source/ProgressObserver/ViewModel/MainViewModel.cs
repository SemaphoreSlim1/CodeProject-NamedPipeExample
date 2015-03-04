using GalaSoft.MvvmLight;
using NamedPipe;
using ProgressObserver.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace ProgressObserver.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Receiver Property

        /// <summary>
        /// Member-level variable backing <see cref="Receiver" />
        /// </summary>
        private Receiver _Receiver = null;

        /// <summary>
        /// Gets and sets the <see cref="Receiver" /> property.         
        /// </summary>
        public Receiver Receiver
        {
            get { 
                if(_Receiver == null)
                { _Receiver = new Receiver(this.OnMessageReceived); }

                return _Receiver; 
            }
            set
            {                
                Set(() => Receiver, ref _Receiver, value);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageItem>();
            Receiver.ServiceOn();
        }

        #region Messages

        public ObservableCollection<MessageItem> Messages { get; set; }

        #endregion

        private void OnMessageReceived(String message)
        {            
            Dispatcher.CurrentDispatcher.Invoke(() => {
                Messages.Add(new MessageItem(message));
            });
        }
    }
}