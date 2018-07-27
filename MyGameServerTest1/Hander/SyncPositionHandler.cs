using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
using Common.Tools;

namespace MyGameServerTest1.Hander
{
    class SyncPositionHandler : BaseHandler
    {
        public SyncPositionHandler()
        {
            opCode = OperationCode.SyncPosition;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //接收位置并保持起来
            float x = (float)DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.x);
            float y = (float)DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.y);
            float z = (float)DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.z);

            //把位置数据传递给Clientpeer保存管理起来
            peer.x = x;
            peer.y = y;
            peer.z = z;

            MyGameServer.log.Info(x + "--" + y + "--" + z);//输出测试
        }
    }
}
