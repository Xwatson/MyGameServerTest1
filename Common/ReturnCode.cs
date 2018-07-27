using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ReturnCode : short //区分请求返回值，成功或者失败
    {
        Success,
        Failed,
        LoginRepeat // 重复登录
    }
}
