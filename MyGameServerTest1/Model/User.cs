using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServerTest1.Model
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }//用户名
        public virtual string Password { get; set; }//密码
        public virtual DateTime CreatedTime { get; set; }//注册时间
    }
}
