using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Classes
{
  public sealed  class Multiton
    {
        
            static Dictionary<int, Multiton> _cameras = new Dictionary<int, Multiton>();
            static object _lock = new object();

            private Multiton(TcpListener myList)
            {
                Socet = myList.AcceptSocket();
        }

            public static Multiton GetSocket(int cameraCode)
            {
                lock (_lock)
                {
                    if (!_cameras.ContainsKey(cameraCode)) _cameras.Add(cameraCode, new Multiton(Singleton.GetSingleton()));
                }
                return _cameras[cameraCode];
            }

            public Socket Socet { get; private set; }
        
    }
}
