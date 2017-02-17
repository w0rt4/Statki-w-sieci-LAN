using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Classes
{
   public class Proxy : IConnection
    {
        TcpIP tcpclnt = new TcpIP();


        public void Connect(string ip)
        {
            try
            {
                tcpclnt.Connect(ip); //"192.168.1.103", 8081
            }
            catch (Exception e)
            {
                //TODO
            }
        }

        public void Send(string str)
        {

            tcpclnt.Send(str);
        }

        public byte[] Recived()
        {
            return tcpclnt.Recived();

        }

        public void Close()
        {
            tcpclnt.Close();
        }

       
    }
}
