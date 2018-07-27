using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using MyGameServerTest1.Hander;
using Common.Tools;
using Common;
using ExitGames.Logging;

namespace MyGameServerTest1
{
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public string username;
        public static readonly ILogger log = LogManager.GetCurrentClassLogger();
        public float x, y, z;

        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {

        }
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            MyGameServer.Instance.peerlist.Remove(this);//断开连接的时候List里面移除当前的ClientPeer客户端
            log.Info("用户：" + username + " 已退出！剩余客户端：" + MyGameServer.Instance.peerlist.Count);
            //告诉其他客户端需要销毁的客户端
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerlist)
            {
                EventData ed = new EventData((byte)EventCode.DestroyPlayer);
                Dictionary<byte, object> data2 = new Dictionary<byte, object>();
                data2.Add((byte)ParameterCode.Username, username);
                ed.Parameters = data2;
                tempPeer.SendEvent(ed, new SendParameters());//发送事件
            }
        }
        //处理客户端的请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            log.Info("获取到请求：" + (OperationCode)operationRequest.OperationCode);
            //OperationRequest封装了请求的信息
            //SendParameters 参数，客户端向服务器传递过来的数据

            //通过客户端的OperationCode从HandlerDict里面获取到了需要的Hander
            BaseHandler handler = DictTool.GetValue<OperationCode, BaseHandler>(MyGameServer.Instance.HandlerDict, (OperationCode)operationRequest.OperationCode);
            //如果找到了需要的hander就调用我们hander里面处理请求的方法
            if (handler != null)
            {
                handler.OnOperationRequest(operationRequest, sendParameters, this);

            }
            else//否则我们就使用默认的hander
            {
                BaseHandler defaultHandler = DictTool.GetValue<OperationCode, BaseHandler>(MyGameServer.Instance.HandlerDict, OperationCode.Default);
                defaultHandler.OnOperationRequest(operationRequest, sendParameters, this);
            }
            
        }
    }
}
