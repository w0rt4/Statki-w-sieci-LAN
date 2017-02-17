using System;
using System.IO;
using System.Net.Sockets;
using Server.Classes;


namespace Client2
{
    class Client2
    {
        static void Main(string[] args)
        {
            try
            {

                IConnection proxy = new Proxy();
                Console.WriteLine("Connecting.....");
                proxy.Connect("192.168.1.103");

                // use the ipaddress as in the server program

                Console.WriteLine("Connected");
              //  Console.Write("Enter the string to be transmitted : ");

               // String str = Console.ReadLine();

              //  proxy.Send(str);

            

                byte[] bb = new byte[150];
                bb = proxy.Recived();

                for (int i = 0; i < bb.Length; i++)
                    Console.Write(Convert.ToChar(bb[i]));
                
                proxy.Close();
                Console.Read();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}
