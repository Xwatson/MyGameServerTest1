using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;

namespace MyGameServerTest1.Hander
{
    class DefaultHandler : BaseHandler
    {
        public DefaultHandler()
        {
            opCode = OperationCode.Default;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
        }
    }
}
