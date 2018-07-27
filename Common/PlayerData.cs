using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [Serializable]
    public class PlayerData
    {
        public Vector3Data pos { get; set; }
        public string Username { get; set; }
    }
}
