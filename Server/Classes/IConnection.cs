using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Classes
{
  public  interface IConnection
    {
        void Connect(string ip);
        void Send(string str);
        byte[] Recived();
        void Close();
    }
}
