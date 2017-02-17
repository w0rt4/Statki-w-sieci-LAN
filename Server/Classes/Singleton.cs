using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Classes
{

    public sealed class Singleton
    {
        private static TcpListener _instance;
        private static object _lockThis = new object();

        private Singleton()
        {

        }

        public static TcpListener GetSingleton()
        {
            lock (_lockThis)
            {
                if (_instance == null)
                {
                    IPAddress ipAd = IPAddress.Parse("192.168.1.103");
                    _instance = new TcpListener(ipAd, 8001);
                    _instance.Start();
                }
            }

            return _instance;
        }
        ~Singleton()
        {
            _instance.Stop();
        }

    }
}
