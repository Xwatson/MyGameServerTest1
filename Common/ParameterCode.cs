using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ParameterCode : byte //区分传送数据时候参数的类型
    {
        Username,
        Password,
        Position,
        x, y, z,
        UsernameList,
        PlayerDataList
    }
}
