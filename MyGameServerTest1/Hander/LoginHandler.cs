using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
using Common.Tools;
using MyGameServerTest1.Manager;

namespace MyGameServerTest1.Hander
{
    class LoginHandler : BaseHandler
    {
        public LoginHandler()
        {
            opCode = OperationCode.Login;//赋值OperationCode为Login,表示处理的是那个请求
        }
        //登陆请求的处理的代码
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //根据发送过来的请求获得用户名和密码
            string username = DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Username) as string;
            string password = DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Password) as string;
            //连接数据库进行校验
            UserManager manager = new UserManager();
            bool isSuccess = manager.VerifyUser(username, password);
            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            //如果验证成功，把成功的结果利用response.ReturnCode返回成功给客户端
            if (isSuccess)
            {
                response.ReturnCode = (short)ReturnCode.Success;
                peer.username = username;
                bool isLoginRepeat = false;
                foreach (ClientPeer cPeer in MyGameServer.Instance.peerlist)
                {
                    // 已登录
                    if (cPeer.username == username)
                    {
                        isLoginRepeat = true;
                        response.ReturnCode = (short)ReturnCode.LoginRepeat;
                        break;
                    }
                }
                if (!isLoginRepeat)
                {
                    MyGameServer.log.Info("用户：" + username + " 已登录");
                    MyGameServer.Instance.peerlist.Add(peer);
                }
            }
            else//否则返回失败给客户端
            {
                response.ReturnCode = (short)ReturnCode.Failed;
            }
            //把上面的回应给客户端
            peer.SendOperationResponse(response, sendParameters);

        }
    }
}
