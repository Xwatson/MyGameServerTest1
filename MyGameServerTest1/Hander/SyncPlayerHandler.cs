using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
using System.IO;
using System.Xml.Serialization;

namespace MyGameServerTest1.Hander
{
    public class SyncPlayerHandler : BaseHandler
    {
        public SyncPlayerHandler()
        {
            opCode = OperationCode.SyncPlayer;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //取得所有已经登陆（在线玩家）的用户名
            List<string> usernameList = new List<string>();

            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerlist)
            {
                //string.IsNullOrEmpty(tempPeer.username);//如果用户名为空表示没有登陆
                //如果连接过来的客户端已经登陆了有用户名了并且这个客户端不是当前的客户端
                if (string.IsNullOrEmpty(tempPeer.username) == false && tempPeer != peer)
                {
                    //把这些客户端的Usernam添加到集合里面
                    usernameList.Add(tempPeer.username);
                }
            }

            //通过xml序列化进行数据传输,传输给客户端
            StringWriter sw = new StringWriter();
            XmlSerializer serlizer = new XmlSerializer(typeof(List<string>));
            serlizer.Serialize(sw, usernameList);
            sw.Close();
            string usernameListString = sw.ToString();
            //给客户端响应
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.UsernameList, usernameListString);
            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            response.Parameters = data;
            peer.SendOperationResponse(response, sendParameters);
            //告诉其他客户端有新的客户端加入
            foreach (ClientPeer tempPeer in MyGameServer.Instance.peerlist)
            {
                if (string.IsNullOrEmpty(tempPeer.username) == false && tempPeer != peer)
                {
                    EventData ed = new EventData((byte)EventCode.NewPlayer);
                    Dictionary<byte, object> data2 = new Dictionary<byte, object>();
                    data2.Add((byte)ParameterCode.Username, peer.username);//把新进来的用户名传递给其他客户端
                    ed.Parameters = data2;
                    tempPeer.SendEvent(ed, sendParameters);//发送事件
                }
            }
        }
    }
}
