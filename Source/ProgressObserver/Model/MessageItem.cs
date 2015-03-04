using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressObserver.Model
{
    public class MessageItem
    {
        public MessageItem(String msg)
        {
            this.Recieved = DateTime.UtcNow;
            this.Message = msg;
        }

        public DateTime Recieved { get; set; }
        public String Message { get; set; }
    }
}
