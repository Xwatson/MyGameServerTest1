using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;

namespace MyGameServerTest1.Hander
{
    class SyncDestroyPlayerHandler : BaseHandler
    {
        public SyncDestroyPlayerHandler()
        {
            opCode = OperationCode.SyncDestroyPlayer;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //告诉其他客户端销毁退出的客户端
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerlist)
            {
                if (string.IsNullOrEmpty(tempPeer.username) == false && tempPeer != peer)
                {
                    EventData ed = new EventData((byte)EventCode.DestroyPlayer);
                    Dictionary<byte, object> data2 = new Dictionary<byte, object>();
                    data2.Add((byte)ParameterCode.Username, peer.username);//把新进来的用户名传递给其他客户端
                    ed.Parameters = data2;
                    tempPeer.SendEvent(ed, sendParameters);//发送事件
                }
            }
        }
    }
}
