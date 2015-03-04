using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipe.Communication
{
    [ServiceContract]
    internal interface IPipeService
    {
        [OperationContract]
        void RecieveMessage(String message);
    }
}
