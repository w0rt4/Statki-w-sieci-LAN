using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Server.Classes;


namespace Server
{
    public class Server
    {
        public static PlayerMatrix player1Matrix = new PlayerMatrix();
        public static PlayerMatrix player2Matrix = new PlayerMatrix();
        public static int counter = 0;

        public static void Main(string[] args)
        {
            try
            {
                //  IPAddress ipAd = IPAddress.Parse("192.168.1.100");
                // use local m/c IP address, and 
                // use the same in the client

                /* Initializes the Listener */
                // TcpListener myList = new TcpListener(ipAd, 8001);



                /* Start Listeneting at the specified port */
                //  myList.Start();


                Console.WriteLine("The server is running at port 8001...");
                Console.WriteLine("The local End point is  :" +
                                  Singleton.GetSingleton().LocalEndpoint);
                Console.WriteLine("Waiting for a connection.....");

                //otwacie 2 soketów dla 2 graczy
                Multiton s = Multiton.GetSocket(1);
                Multiton s2 = Multiton.GetSocket(2);

                //informacja o połączeniu obydwu graczy
                Console.WriteLine("Connection accepted from " + s.Socet.RemoteEndPoint);
                Console.WriteLine("Connection accepted from " + s2.Socet.RemoteEndPoint);


                //tworzenie obiektu do konwersji danych i wysłanie graczom informacji o starcie gry
                ASCIIEncoding asen = new ASCIIEncoding();
                s.Socet.Send(asen.GetBytes("STRT000"));
                s2.Socet.Send(asen.GetBytes("STRT000"));


                Console.WriteLine("\nSent Acknowledgement");


                byte[] b = new byte[150];
                byte[] b2 = new byte[150];

                //Oczekiwanie na wysłanie przez graczy macierzy 
                s.Socet.Receive(b);
                s2.Socet.Receive(b2);

                //przepisanie macierzy 1 gracza do pamięci
                Message msg = new Message(b);
                msg.getNext();
                if (msg.flag == "NTAB")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            player1Matrix.playerMatrix[j, i] = msg.tab[(j * 10) + i];
                            Console.Write(msg.tab[(j * 10) + i]);
                        }
                        Console.Write("\n");
                    }

                }

                Console.Write("\n\n");
                //przepisanie macierzy 2 gracza do pamięci
                msg = new Message(b2);
                msg.getNext();
                if (msg.flag == "NTAB")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            player2Matrix.playerMatrix[j, i] = msg.tab[(j * 10) + i];
                            Console.Write(msg.tab[(j * 10) + i]);
                        }
                        Console.Write("\n");
                    }

                }


                //wysłanie graczom informacji o kolejności strzałów
                s.Socet.Send(asen.GetBytes("TURN000"));
                s2.Socet.Send(asen.GetBytes("STOP000"));
                s.Socet.Receive(b);
                Console.Write("1 Gracz Gotowy\n");
                s2.Socet.Receive(b);
                Console.Write("2 Gracz Gotowy\n");
                bool turn = true;

                while (true)
                {
                    Socket socket;
                    if (turn)
                    {
                        socket = s.Socet;
                    }
                    else
                    {
                        socket = s2.Socet; //s2
                    }

                    socket.Receive(b);

                    msg = new Message(b);
                    msg.getNext();

                    int x, y;
                    x = msg.tab[1];
                    y = msg.tab[0];

                    if (turn)
                    {
                        //player2Matrix;
                        //strzały 
                        //wynik strzału
                        //response send turn or stop
                        Shoot(player2Matrix, x, y);
                        if (player2Matrix.playerMatrix[x, y] != 123)
                        {

                            Console.Write("Gracz 1 Strzelił w " + x + "," + y + " trafiając\n");

                            //trafił
                            socket.Send(asen.GetBytes("RST1005" + x + y + player2Matrix.playerMatrix[x, y])); //trafiłeś w tą pozycję x y i wynik trafineia 123 lub 999
                            s2.Socet.Send(asen.GetBytes("RST2005" + x + y + player2Matrix.playerMatrix[x, y]));

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s2.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //jeszcze dla obu wiadomosci wysyłamy SUR1/2 dlugosc i x,y na których są 999  do tego potrzebna metoda
                            int[] tab = SUR(player2Matrix);
                            string message = "";

                            for (int i = 0; i < counter * 2; i++)
                            {
                                message += tab[i];
                            }
                            if (counter < 10)
                            {
                                s2.Socet.Send(asen.GetBytes("SUR200" + counter + message));
                                socket.Send(asen.GetBytes("SUR100" + counter + message));
                                Console.Write("SUR200" + counter + message + "\n");
                                Console.Write("SUR100" + counter + message + "\n");
                            }
                            else
                            {
                                s2.Socet.Send(asen.GetBytes("SUR20" + counter + message));
                                socket.Send(asen.GetBytes("SUR10" + counter + message));
                                Console.Write("SUR20" + counter + message + "\n");
                                Console.Write("SUR10" + counter + message + "\n");
                            }

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s2.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //jeszcze dla obu wiadomosci wysyłamy MIS1/2 dlugosc i x,y na których są 123  do tego potrzebna metoda
                            tab = MIS(player2Matrix);
                            message = "";

                            for (int i = 0; i < counter * 2; i++)
                            {
                                message += tab[i];
                            }

                            if (counter < 10)
                            {
                                s2.Socet.Send(asen.GetBytes("MIS200" + counter + message));
                                socket.Send(asen.GetBytes("MIS100" + counter + message));
                                Console.Write("MIS200" + counter + message + "\n");
                                Console.Write("MI1200" + counter + message + "\n");

                            }
                            else
                            {
                                s2.Socet.Send(asen.GetBytes("MIS20" + counter + message));
                                socket.Send(asen.GetBytes("MIS10" + counter + message));
                                Console.Write("MIS20" + counter + message + "\n");
                                Console.Write("MI120" + counter + message + "\n");
                            }

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s2.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //sprawdzenie czy przypadkiem nie jest to ostatnie trafienie jeśli tak to wyślij informacje o końcu i zwycięzcy a jezeli nie każ kontynuować
                            if (!End(player2Matrix))
                            {
                                socket.Send(asen.GetBytes("TURN000"));
                                s2.Socet.Send(asen.GetBytes("STOP000"));
                                Console.Write("S2 - STOP000\n");
                                Console.Write("S1 - TURN000\n");
                            }
                            else
                            {
                                socket.Send(asen.GetBytes("END1000"));
                                s2.Socet.Send(asen.GetBytes("END0000"));
                                Console.Write("S1 - END1000\n");
                                Console.Write("S2 - END0000\n");
                                break;
                            }

                        }
                        else
                        {
                            Console.Write("Gracz 1 Strzelił w " + x + "," + y + " pudłując\n");

                            //pudło
                            socket.Send(asen.GetBytes("RST1005" + x + y + player2Matrix.playerMatrix[x, y])); // wynik 123
                            s2.Socet.Send(asen.GetBytes("RST2005" + x + y + player2Matrix.playerMatrix[x, y]));

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s2.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //jeszcze dla obu wiadomosci wysyłamy SUR1/2000
                            s2.Socet.Send(asen.GetBytes("SUR2000"));
                            socket.Send(asen.GetBytes("SUR1000"));
                            Console.Write("SUR2000\n");
                            Console.Write("SUR1000\n");

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s2.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //jeszcze dla obu wiadomosci wysyłamy MIS1/2000
                            s2.Socet.Send(asen.GetBytes("MIS2000"));
                            socket.Send(asen.GetBytes("MIS1000"));
                            Console.Write("MIS2000\n");
                            Console.Write("MI12000\n");

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s2.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //zmiana tury na przeciwną (2 gracz ma ruch) i wysłanie do graczy odpowiedniej wiadomosci
                            turn = false;
                            socket.Send(asen.GetBytes("STOP000"));
                            s2.Socet.Send(asen.GetBytes("TURN000"));
                            Console.Write("S1 - STOP000\n");
                            Console.Write("S2 - TURN000\n");


                        }
                    }
                    else
                    {

                        Shoot(player1Matrix, x, y);
                        if (player1Matrix.playerMatrix[x, y] != 123)
                        {
                            Console.Write("Gracz 2 Strzelił w " + x + "," + y + " trafiając\n");

                            //2trafił
                            socket.Send(asen.GetBytes("RST1005" + x + y + player1Matrix.playerMatrix[x, y])); //trafiłeś w tą pozycję x y i wynik trafineia 123 lub 999
                            s.Socet.Send(asen.GetBytes("RST2005" + x + y + player1Matrix.playerMatrix[x, y]));

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //jeszcze dla obu wiadomosci wysyłamy SUR1/2 dlugosc i x,y na których są 999 do tego potrzebna metoda
                            int[] tab = SUR(player1Matrix);
                            string message = "";

                            for (int i = 0; i < counter * 2; i++)
                            {
                                message += tab[i];
                            }

                            if (counter < 10)
                            {
                                s.Socet.Send(asen.GetBytes("SUR200" + counter + message));
                                socket.Send(asen.GetBytes("SUR100" + counter + message));
                                Console.Write("SUR200" + counter + message + "\n");
                                Console.Write("SUR100" + counter + message + "\n");
                            }
                            else
                            {
                                socket.Send(asen.GetBytes("SUR10" + counter + message));
                                s.Socet.Send(asen.GetBytes("SUR20" + counter + message));
                            }

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //jeszcze dla obu wiadomosci wysyłamy MIS1/2 dlugosc i x,y na których są 123  do tego potrzebna metoda
                            tab = MIS(player1Matrix);
                            message = "";

                            for (int i = 0; i < counter * 2; i++)
                            {
                                message += tab[i];
                            }

                            if (counter < 10)
                            {
                                s.Socet.Send(asen.GetBytes("MIS200" + counter + message));
                                socket.Send(asen.GetBytes("MIS100" + counter + message));
                                Console.Write("MIS200" + counter + message + "\n");
                                Console.Write("MI1200" + counter + message + "\n");
                            }
                            else
                            {
                                s.Socet.Send(asen.GetBytes("MIS20" + counter + message));
                                socket.Send(asen.GetBytes("MIS10" + counter + message));
                                Console.Write("MIS20" + counter + message + "\n");
                                Console.Write("MI120" + counter + message + "\n");
                            }

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            if (!End(player1Matrix))
                            {
                                socket.Send(asen.GetBytes("TURN000"));
                                s.Socet.Send(asen.GetBytes("STOP000"));
                                Console.Write("S1 - STOP000\n");
                                Console.Write("S2 - TURN000\n");
                            }
                            else
                            {
                                socket.Send(asen.GetBytes("END1000"));
                                s.Socet.Send(asen.GetBytes("END0000"));
                                Console.Write("S2 - END1000\n");
                                Console.Write("S1- END0000\n");
                                break;
                            }


                        }
                        else
                        {
                            Console.Write("Gracz 2 Strzelił w " + x + "," + y + " pudłując\n");

                            //2 pudło
                            socket.Send(asen.GetBytes("RST1005" + x + y + player1Matrix.playerMatrix[x, y])); // wynik 123
                            s.Socet.Send(asen.GetBytes("RST2005" + x + y + player1Matrix.playerMatrix[x, y]));

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //jeszcze dla obu wiadomosci wysyłamy SUR1/2000
                            s.Socet.Send(asen.GetBytes("SUR2000"));
                            socket.Send(asen.GetBytes("SUR1000"));
                            Console.Write("SUR2000\n");
                            Console.Write("SUR1000\n");

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //jeszcze dla obu wiadomosci wysyłamy MIS1/2000
                            s.Socet.Send(asen.GetBytes("MIS2000"));
                            socket.Send(asen.GetBytes("MIS1000"));
                            Console.Write("MIS2000\n");
                            Console.Write("MI12000\n");

                            //klienci potwierdzają dotychczasowy odbiór
                            socket.Receive(b);
                            s.Socet.Receive(b);
                            Console.Write("ACCEPT\n");

                            //zmiana tury na przeciwną (2 gracz ma ruch) i wysłanie do graczy odpowiedniej wiadomosci
                            turn = true;
                            socket.Send(asen.GetBytes("STOP000"));
                            s.Socet.Send(asen.GetBytes("TURN000"));
                            Console.Write("S2 - STOP000\n");
                            Console.Write("S1 - TURN000\n");

                        }
                    }


                }



                /* clean up */
                s.Socet.Close();
                s2.Socet.Close();

                Console.Read();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
                Console.Read();
            }
        }

        public static PlayerMatrix Shoot(PlayerMatrix pM, int x, int y)
        {

            int tx = x;
            int ty = y;
            if (pM.playerMatrix[x, y] == 1 || pM.playerMatrix[x, y] == 3 || pM.playerMatrix[x, y] == 5 || pM.playerMatrix[x, y] == 7)
            {
                if (pM.playerMatrix[x, y] == 1)
                {
                    pM.playerMatrix[x, y] = 999; // zatopiony

                    int truey = y;
                    int truex = x;
                    int rotate = 2; // pozioma
                    int lenght = 0;


                    y--; //zatapiamy pozostałe maszty
                    if (y >= 0)
                        while (pM.playerMatrix[x, y] != 9 && pM.playerMatrix[x, y] != 18 && pM.playerMatrix[x, y] != 123)
                        {
                            rotate = 2;//poziom
                            pM.playerMatrix[x, y] = 999;
                            y--;
                            if (y < 0)
                                break;
                        }
                    y = truey + 1;
                    if (y <= 9)
                        while (pM.playerMatrix[x, y] != 9 && pM.playerMatrix[x, y] != 18 && pM.playerMatrix[x, y] != 123)
                        {
                            rotate = 2;//poziom
                            pM.playerMatrix[x, y] = 999;
                            y++;
                            if (y > 9)
                                break;
                        }
                    x--;
                    y = truey;
                    if (x >= 0)
                        while (pM.playerMatrix[x, y] != 9 && pM.playerMatrix[x, y] != 18 && pM.playerMatrix[x, y] != 123)
                        {
                            rotate = 1;//pionowa
                            pM.playerMatrix[x, y] = 999;
                            x--;
                            if (x < 0)
                                break;
                        }
                    x = truex + 1;
                    if (x <= 9)
                        while (pM.playerMatrix[x, y] != 9 && pM.playerMatrix[x, y] != 18 && pM.playerMatrix[x, y] != 123)
                        {
                            rotate = 1;//pionowa
                            pM.playerMatrix[x, y] = 999;
                            x++;
                            if (x > 9)
                                break;
                        }

                    x = truex;
                    y = truey;
                    if (rotate == 1) //pionowa
                    {

                        while (x > 0 && pM.playerMatrix[x, y] == 999)
                        {
                            if (pM.playerMatrix[x - 1, y] == 999)
                            {
                                truex = x - 1;
                            }
                            x--;

                        }
                        x = truex;
                        while (x < 9 && pM.playerMatrix[x, y] == 999)
                        {
                            x++;
                            lenght++;
                        }
                    }
                    else //poziom
                    {
                        while (y > 0 && pM.playerMatrix[x, y] == 999)
                        {
                            if (pM.playerMatrix[x, y - 1] == 999)
                            {
                                truey = y - 1;
                            }
                            y--;

                        }
                        y = truey;
                        while (y < 9 && pM.playerMatrix[x, y] == 999)
                        {
                            y++;
                            lenght++;
                        }
                    }

                    if (rotate == 1)
                    {
                        for (int i = truey - 1; i <= truey + 1; i++)
                            for (int j = truex - 1; j <= truex + lenght; j++)
                            {
                                if (i < 10 && j < 10 && i >= 0 && j >= 0)
                                    if (pM.playerMatrix[j, i] != 999) // JA JEBIE XD
                                        pM.playerMatrix[j, i] = 123;
                            }
                    }
                    else
                    {
                        for (int i = truex - 1; i <= truex + 1; i++)
                            for (int j = truey - 1; j <= truey + lenght; j++)
                            {
                                if (i < 10 && j < 10 && i >= 0 && j >= 0)
                                    if (pM.playerMatrix[i, j] != 999)
                                        pM.playerMatrix[i, j] = 123;
                            }
                    }


                }
                else
                {
                    pM.playerMatrix[x, y] = 321; //trafiony

                    int truey = y;
                    int truex = x;

                    y--;
                    if (y >= 0)   //odejmujemy stałą 2 od pozostałych masztów
                        while (pM.playerMatrix[x, y] != 9 && pM.playerMatrix[x, y] != 18 && pM.playerMatrix[x, y] != 123)
                        {
                            if (pM.playerMatrix[x, y] != 321)
                                pM.playerMatrix[x, y] -= 2;
                            y--;
                            if (y < 0)
                                break;
                        }
                    y = truey + 1;
                    if (y <= 9)
                        while (pM.playerMatrix[x, y] != 9 && pM.playerMatrix[x, y] != 18 && pM.playerMatrix[x, y] != 123)
                        {
                            if (pM.playerMatrix[x, y] != 321)
                                pM.playerMatrix[x, y] -= 2;
                            y++;
                            if (y > 9)
                                break;
                        }
                    x--;
                    y = truey;
                    if (x >= 0)
                        while (pM.playerMatrix[x, y] != 9 && pM.playerMatrix[x, y] != 18 && pM.playerMatrix[x, y] != 123)
                        {
                            if (pM.playerMatrix[x, y] != 321)
                                pM.playerMatrix[x, y] -= 2;
                            x--;
                            if (x < 0)
                                break;
                        }
                    x = truex + 1;
                    if (x <= 9)
                        while (pM.playerMatrix[x, y] != 9 && pM.playerMatrix[x, y] != 18 && pM.playerMatrix[x, y] != 123)
                        {
                            if (pM.playerMatrix[x, y] != 321)
                                pM.playerMatrix[x, y] -= 2;
                            x++;
                            if (x > 9)
                                break;
                        }


                }

            }
            else
             if (pM.playerMatrix[x, y] != 321 && pM.playerMatrix[x, y] != 999)
            {

                pM.playerMatrix[x, y] = 123;
            }

            return pM;
        }

        public static bool End(PlayerMatrix pM)
        {
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (pM.playerMatrix[i, j] == 999)
                    {
                        count++;
                    }
                }
            }
            if (count == 20)
            {
                Console.Write("KONIEC GRY\n");
                return true;
            }
            return false;
        }

        public static int[] SUR(PlayerMatrix pM)
        {
            counter = 0;
            int[] tab = new int[100];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (pM.playerMatrix[i, j] == 999)
                    {

                        tab[(counter * 2)] = i;
                        tab[1 + (2 * counter)] = j;
                        counter++;
                    }
                }
            }
            return tab;
        }

        public static int[] MIS(PlayerMatrix pM)
        {
            counter = 0;
            int[] tab = new int[160];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (pM.playerMatrix[i, j] == 123)
                    {

                        tab[(counter * 2)] = i;
                        tab[1 + (2 * counter)] = j;
                        counter++;
                    }
                }
            }
            return tab;
        }

    }
}

