using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Classes
{
    public class TcpIP : IConnection
    {

        TcpClient tcpclnt = new TcpClient();

        public void Close()
        {
            tcpclnt.Close();
        }

        public void Connect(string ip)
        {
            try
            {
                tcpclnt.Connect(ip,8001); //"192.168.1.103" , 8001
            }
            catch (Exception e)
            {
                //TODO
            }
        }

        public byte[] Recived()
        {
            byte[] bb = new byte[200];
            try
            {

                int k = tcpclnt.GetStream().Read(bb, 0, 200);
                return bb;
            }
            catch (Exception e)
            {
                //TODO
                return bb;
            }
        }

        public void Send(string str)
        {
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                tcpclnt.GetStream().Write(ba, 0, ba.Length);
            }
            catch (Exception e)
            {
                //TODO
            }
        }
    }
}
