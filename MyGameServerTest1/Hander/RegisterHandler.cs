using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using Common;
using Common.Tools;
using MyGameServerTest1.Manager;
using MyGameServerTest1.Model;

namespace MyGameServerTest1.Hander
{
    class RegisterHandler : BaseHandler
    {
        public RegisterHandler()
        {
            opCode = OperationCode.Register;
        }
        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string username = DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Username) as string;
            string password = DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.Password) as string;
            UserManager manager = new UserManager();
            User user = manager.GetByUsername(username);//根据username查询数据
            OperationResponse responser = new OperationResponse(operationRequest.OperationCode);
            //如果没有查询到代表这个用户没被注册过可用
            if (user == null)
            {
                //添加输入的用户和密码进数据库
                user = new User() { Username = username, Password = password };
                manager.Add(user);
                responser.ReturnCode = (short)ReturnCode.Success;//返回成功

            }
            else//否者这个用户被注册了
            {
                responser.ReturnCode = (short)ReturnCode.Failed;//返回失败
            }
            // 把上面的结果给客户端
            peer.SendOperationResponse(responser, sendParameters);
            
        }
    }
}
