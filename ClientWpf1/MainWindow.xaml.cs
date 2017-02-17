using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientWpf1.Content;
using Server.Classes;

namespace ClientWpf1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Image[,] playerGameBoard = new Image[10, 10];
        public Image[,] enemyGameBoard = new Image[10, 10];
        public PlayerMatrix playerMatrix;
        public PlayerMatrix enemyMatrix;
        public Ship s;
        public int x = 0;
        public int y = 0;
        public int strtn = 0;
        public int turns = 0;
        public ImageBuilder builder;
        public PointerMover strategy;
        IConnection proxy;

        public Image imgL = new Image
        {
            Source = Const.imgLogo,
            Stretch = Stretch.Fill
        };
        public MainWindow()
        {
            InitializeComponent();
            Grid.SetColumn(imgL, 1);
            Grid.SetColumnSpan(imgL, 5);
            Grid.SetRow(imgL, 0);
            GridStart.Children.Add(imgL);

        }

        private void StartTcpGame(object sender, RoutedEventArgs e)
        {
            try
            {

                proxy = new Proxy();
                Console.WriteLine("Connecting.....");
                proxy.Connect("192.168.1.103"); // use the ipaddress as in the server program
                Console.WriteLine("Connected");

              //  String str = Console.ReadLine();
             //   proxy.Send(str);



                byte[] bb = new byte[200];
                bb = proxy.Recived();
                Message msg = Methods.ReadMessage(bb);

               // Console.Write(Convert.ToChar(bb[i]));

                GridStart.Visibility = Visibility.Hidden;
                

                tb.Text = msg.flag;

                if (msg.flag == "STRT")
                {
                    GridStart.Visibility = Visibility.Hidden;
                    PlaceShipScreen.Visibility = Visibility.Visible;
                    playerMatrix = new PlayerMatrix();
                    enemyMatrix = new PlayerMatrix();
                    builder = new ImageBuilder();
                    strategy = new PointerMover();
                    PlaceShip(4, playerMatrix = new PlayerMatrix(), false);
                }
                else
                {
                    proxy.Close();
                    Console.Read();
                }
            }

            catch (Exception error)
            {
                Console.WriteLine("Error..... " + error.StackTrace);
            }
        }

        public PlayerMatrix PlaceShip(int size, PlayerMatrix pM, bool player)
        {
            s = new Ship(size);

            pM = SetShip(pM, s);
            if (!CheckAllPosition(pM)) // jeżeli okazuje się że są nakładki
            {
                AcceptButton.IsEnabled = false;
            }
            else
            {
                AcceptButton.IsEnabled = true;
            }
            CreateScreen(pM);
            return pM;


        }

        public void CreateScreen(PlayerMatrix pM)
        {
            MainWindow1.Height = 600;
            MainWindow1.Width = 600;
            GridStart.Visibility = Visibility.Hidden;
            PlaceShipScreen.Visibility = Visibility.Visible;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (pM.playerMatrix[i, j] == 0)
                    {
                        builder.BuildWather();
                        playerGameBoard[i, j] = builder.GetImage();
                    }
                    else if (pM.playerMatrix[i, j] == 9)
                    {
                        builder.BuildMiss();
                        playerGameBoard[i, j] = builder.GetImage();
                    }
                    else if (pM.playerMatrix[i, j] == 1 || pM.playerMatrix[i, j] == 3 || pM.playerMatrix[i, j] == 5 || pM.playerMatrix[i, j] == 7)
                    {
                        builder.BuildShip();
                        playerGameBoard[i, j] = builder.GetImage();
                    }
                    else
                    {
                        builder.BuildColide();
                        playerGameBoard[i, j] = builder.GetImage();
                    }


                    Grid.SetColumn(playerGameBoard[i, j], i + 1);
                    Grid.SetRow(playerGameBoard[i, j], j + 1);
                    PlaceShipScreen.Children.Add(playerGameBoard[i, j]);
                }
            }
        }

        public static PlayerMatrix SetShip(PlayerMatrix pM, Ship s)
        {
            if (s.rotate)
                for (int i = s.y; i < s.y + s.size; i++)
                    pM.playerMatrix[i, s.x] += (s.size * 2) - 1;
            else
                for (int j = s.x; j < s.x + s.size; j++)
                    pM.playerMatrix[s.y, j] += (s.size * 2) - 1;
            return pM;
        }

        public static PlayerMatrix ClearShip(PlayerMatrix pM, Ship s)
        {
            if (s.rotate)
                for (int i = s.y; i < s.y + s.size; i++)
                    pM.playerMatrix[i, s.x] -= (s.size * 2) - 1;
            else
                for (int j = s.x; j < s.x + s.size; j++)
                    pM.playerMatrix[s.y, j] -= (s.size * 2) - 1;
            return pM;
        }

        public static bool CheckAllPosition(PlayerMatrix pM)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                        if ((pM.playerMatrix[i, j] != 0) && (pM.playerMatrix[i, j] != 1) && (pM.playerMatrix[i, j] != 3) && (pM.playerMatrix[i, j] != 5) && (pM.playerMatrix[i, j] != 7) && (pM.playerMatrix[i, j] != 9))
                            return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void MoveShipRight(object sender, RoutedEventArgs e)
        {
            if ((s.y < 9 && s.rotate == false) || (s.rotate == true && s.y + s.size - 1 < 9))
            {

                ClearShip(playerMatrix, s);
                s.y++;
                SetShip(playerMatrix, s);
                CreateScreen(playerMatrix);
                if (!CheckAllPosition(playerMatrix)) // jeżeli okazuje się że są nakładki
                {
                    AcceptButton.IsEnabled = false;
                }
                else
                {
                    AcceptButton.IsEnabled = true;
                }
            }
            else
            {


                CreateScreen(playerMatrix);

            }

        }

        private void MoveShipLeft(object sender, RoutedEventArgs e)
        {
            if (s.y > 0)
            {

                ClearShip(playerMatrix, s);
                s.y--;
                SetShip(playerMatrix, s);
                CreateScreen(playerMatrix);
                if (!CheckAllPosition(playerMatrix)) // jeżeli okazuje się że są nakładki
                {
                    AcceptButton.IsEnabled = false;
                }
                else
                {
                    AcceptButton.IsEnabled = true;
                }
            }
            else
            {


                CreateScreen(playerMatrix);

            }

        }

        private void MoveShipUp(object sender, RoutedEventArgs e)
        {

            if (s.x > 0)
            {


                ClearShip(playerMatrix, s);
                s.x--;
                SetShip(playerMatrix, s);
                CreateScreen(playerMatrix);
                if (!CheckAllPosition(playerMatrix)) // jeżeli okazuje się że są nakładki
                {
                    AcceptButton.IsEnabled = false;
                }
                else
                {
                    AcceptButton.IsEnabled = true;
                }


            }

            else
            {

                CreateScreen(playerMatrix);


            }

        }

        private void MoveShipDown(object sender, RoutedEventArgs e)
        {
            if ((s.x < 9 && s.rotate == true) || (s.rotate == false && s.x + s.size - 1 < 9))
            {

                ClearShip(playerMatrix, s);
                s.x++;
                SetShip(playerMatrix, s);
                CreateScreen(playerMatrix);
                if (!CheckAllPosition(playerMatrix)) // jeżeli okazuje się że są nakładki
                {
                    AcceptButton.IsEnabled = false;
                }
                else
                {
                    AcceptButton.IsEnabled = true;
                }

            }
            else
            {


                CreateScreen(playerMatrix);

            }

        }

        private void RotateShip(object sender, RoutedEventArgs e)
        {
            if ((s.rotate == false && s.y + s.size - 1 <= 9) || (s.rotate == true && s.x + s.size - 1 <= 9))
            {

                ClearShip(playerMatrix, s);
                s.rotate = !s.rotate;
                SetShip(playerMatrix, s);
                CreateScreen(playerMatrix);
                if (!CheckAllPosition(playerMatrix)) // jeżeli okazuje się że są nakładki
                {
                    AcceptButton.IsEnabled = false;
                }
                else
                {
                    AcceptButton.IsEnabled = true;
                }

            }
            else
            {

                CreateScreen(playerMatrix);

            }

        }

        private void SetShipOnPlace(object sender, RoutedEventArgs e)
        {

            if (Variable2.TRZYMASZTOWCE > 0)
            {
                playerMatrix = SuroundShip(playerMatrix, s);
                Variable2.TRZYMASZTOWCE--;
                PlaceShip(3, playerMatrix, false);
            }
            else
            {
                if (Variable2.DWUMASZTOWCE > 0)
                {
                    playerMatrix = SuroundShip(playerMatrix, s);
                    Variable2.DWUMASZTOWCE--;
                    PlaceShip(2, playerMatrix, false);
                }
                else
                {
                    if (Variable2.JEDNOMASZTOWIEC > 0)
                    {
                        playerMatrix = SuroundShip(playerMatrix, s);
                        Variable2.JEDNOMASZTOWIEC--;
                        PlaceShip(1, playerMatrix, false);
                    }
                    else
                    {

                        playerMatrix = SuroundShip(playerMatrix, s);
                        string msg = "NTAB100";
                        for (int i = 0; i < 10; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                msg += playerMatrix.playerMatrix[i, j];
                            }
                        }
                        proxy.Send(msg);

                        tb.Text = "Oczekiwanie na 2 gracza";

                        //tu czekanie na zezwolenie od servera normalnie
                        byte[] bb = new byte[150];
                        bb = proxy.Recived();
                        Message mssg = Methods.ReadMessage(bb);
                        if (mssg.flag == "TURN")
                        {
                            tb.Text = "Twoja tura";
                            GameScreen();
                            turns = 1;
                            return;
                        }
                        else
                        {
                            tb.Text = "Tura 2 gracza";
                            GameScreen();
                            turns = 0;
                           // ApprovedShot.Visibility = Visibility.Hidden;
                           // enemyTour();
                        }

                    }

                }
            }
        }

        private void enemyTour()
        {
            byte[] bb = new byte[200];
            bb = proxy.Recived();
            Message mssg = Methods.ReadMessage(bb);
            if (mssg.flag == "RST2")
            {
                string temp = "" + mssg.tab[2] + "" + mssg.tab[3] + "" + mssg.tab[4];
                Int32.TryParse(temp, out playerMatrix.playerMatrix[mssg.tab[0], mssg.tab[1]]);
                if (x == mssg.tab[0] && y == mssg.tab[1])
                {
                    playerMatrix.playerMatrix[mssg.tab[0], mssg.tab[1]] += 1000;
                }
            }

            proxy.Send("ACCEPT");

            bb = proxy.Recived();
            mssg = Methods.ReadMessage(bb);
            if (mssg.flag == "SUR2" && mssg.tab != null)
            {
                int temp = 0;
                while (temp < mssg.tab.Length)
                {
                    playerMatrix.playerMatrix[mssg.tab[temp], mssg.tab[temp + 1]] = 999;
                    temp += 2;
                }
            }

            proxy.Send("ACCEPT");

            bb = proxy.Recived();
            mssg = Methods.ReadMessage(bb);
            if (mssg.flag == "MIS2" && mssg.tab != null)
            {
                int temp = 0;
                while (temp < mssg.tab.Length)
                {
                    playerMatrix.playerMatrix[mssg.tab[temp], mssg.tab[temp + 1]] = 123;
                    temp += 2;
                }
            }

            proxy.Send("ACCEPT");

            bb = proxy.Recived();
            mssg = Methods.ReadMessage(bb);
            if (mssg.flag == "END0")
            {
                tb.Text = "Koniec Gry wygrał2 gracz";
                MainWindow1.Height = 300;
                MainWindow1.Width =300;
                Grid1.Visibility = Visibility.Hidden;
                End.Visibility = Visibility.Visible;
                tb.Text = "Koniec Gry wygrał2 gracz";

            }
            else
            {
                if (mssg.flag == "STOP")
                {
                    GameScreen();
                    tb.Text = "Tura Przeciwnika";
                    ApprovedShot.Visibility = Visibility.Hidden;
                    enemyTour();
                    return;
                }
                if (mssg.flag == "TURN")
                {
                    tb.Text = "Twoja tura";
                    ApprovedShot.Visibility = Visibility.Visible;
                    GameScreen();
                }
            }


        }

        public static PlayerMatrix SuroundShip(PlayerMatrix pM, Ship s)
        {
            if (s.rotate)
            {
                for (int i = s.x - 1; i <= s.x + 1; i++)
                    for (int j = s.y - 1; j <= s.y + s.size; j++)
                    {
                        if (i < 10 && j < 10 && i >= 0 && j >= 0)
                            if (pM.playerMatrix[j, i] == 0)
                                pM.playerMatrix[j, i] = 9;
                    }
            }
            else
            {
                for (int i = s.x - 1; i <= s.x + s.size; i++)
                    for (int j = s.y - 1; j <= s.y + 1; j++)
                    {
                        if (i < 10 && j < 10 && i >= 0 && j >= 0)
                            if (pM.playerMatrix[j, i] == 0)
                                pM.playerMatrix[j, i] = 9;
                    }
            }
            return pM;
        }

        private void PointerGoUp(object sender, RoutedEventArgs e)
        {
            strategy.strategy = new MoveUp();
            int[] tab;

            tab = strategy.Move(enemyMatrix, enemyGameBoard, true, x, y, builder);
            x = tab[0];
            if (tab[1] == 1)
            {
                ApprovedShot.IsEnabled = true;
            }
            else
            {
                ApprovedShot.IsEnabled = false;

            }
        } //poruszanie się wskaźnikiem do strzału 

        private void PointerGoDown(object sender, RoutedEventArgs e)
        {

            strategy.strategy = new MoveDown();
            int[] tab;

            tab = strategy.Move(enemyMatrix, enemyGameBoard, true, x, y, builder);
            x = tab[0];
            if (tab[1] == 1)
            {
                ApprovedShot.IsEnabled = true;
            }
            else
            {
                ApprovedShot.IsEnabled = false;
            }



        } //to co wyzej 

        private void PointerGoLeft(object sender, RoutedEventArgs e)
        {


            strategy.strategy = new MoveLeft();
            int[] tab;

            tab = strategy.Move(enemyMatrix, enemyGameBoard, true, x, y, builder);
            y = tab[0];
            if (tab[1] == 1)
            {
                ApprovedShot.IsEnabled = true;
            }
            else
            {
                ApprovedShot.IsEnabled = false;
            }


        } //to co wyzej 

        private void PointerGoRight(object sender, RoutedEventArgs e)
        {


            strategy.strategy = new MoveRight();
            int[] tab;

            tab = strategy.Move(enemyMatrix, enemyGameBoard, true, x, y, builder);
            y = tab[0];
            if (tab[1] == 1)
            {
                ApprovedShot.IsEnabled = true;
            }
            else
            {
                ApprovedShot.IsEnabled = false;
            }

        }//to co wyzej 

        private void ApprovedShotGo(object sender, RoutedEventArgs e)
        {

            proxy.Send("SHOT002" + x + y);
            ApprovedShot.IsEnabled = false;
            byte[] bb = new byte[200];
            bb = proxy.Recived();
            Message mssg = Methods.ReadMessage(bb);
            if (mssg.flag == "RST1")
            {
                string temp = "" + mssg.tab[2] + "" + mssg.tab[3] + "" + mssg.tab[4];
                Int32.TryParse(temp, out enemyMatrix.playerMatrix[mssg.tab[1], mssg.tab[0]]);
                enemyMatrix.playerMatrix[mssg.tab[1], mssg.tab[0]] += 1000;
            }

            proxy.Send("ACCEPT");

            bb = proxy.Recived();
            mssg = Methods.ReadMessage(bb);
            if (mssg.flag == "SUR1" && mssg.tab != null)
            {
                int temp = 0;
                while (temp < mssg.tab.Length)
                {
                    enemyMatrix.playerMatrix[mssg.tab[temp+1], mssg.tab[temp ]] = 999;
                    temp += 2;
                }
            }

            proxy.Send("ACCEPT");

            bb = proxy.Recived();
            mssg = Methods.ReadMessage(bb);
            if (mssg.flag == "MIS1" && mssg.tab != null)
            {
                int temp = 0;
                while (temp < mssg.tab.Length)
                {
                    enemyMatrix.playerMatrix[mssg.tab[temp+1], mssg.tab[temp]] = 123;
                    temp += 2;
                }
            }

            proxy.Send("ACCEPT");

            bb = proxy.Recived();
            mssg = Methods.ReadMessage(bb);
            if (mssg.flag == "END0")
            {
                tb.Text = "Koniec Gry wygrał2 gracz";
                MainWindow1.Height = 300;
                MainWindow1.Width = 300;
                Grid1.Visibility = Visibility.Hidden;
                End.Visibility = Visibility.Visible;
                tb.Text = "Koniec Gry wygrał2 gracz";
            }
            else
            {
                if (mssg.flag == "END1")
                {
                    tb.Text = "Koniec Wygrałeś";
                    MainWindow1.Height = 300;
                    MainWindow1.Width = 300;
                    Grid1.Visibility = Visibility.Hidden;
                    End.Visibility = Visibility.Visible;
                    tb.Text = "Koniec Wygrałeś";
                }

                else
                {
                    if (mssg.flag == "STOP")
                    {
                        
                        tb.Text = "Tura Przeciwnika";
                        GameScreen();
                        enemyTour();
                        return;
                    }
                    if (mssg.flag == "TURN")
                    {
                        tb.Text = "Twoja tura";     
                        GameScreen();
                    }
                }

            }
        }

        public void GameScreen()
        {
            MainWindow1.Height = 600;
            MainWindow1.Width = 800;
            Grid1.Visibility = Visibility.Visible;
            PlaceShipScreen.Visibility = Visibility.Hidden;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (enemyMatrix.playerMatrix[i, j] == 123)
                    {
                        builder.BuildMiss();
                        enemyGameBoard[i, j].Source = builder.GetImage().Source;
                    }
                    else
                    {
                        if (enemyMatrix.playerMatrix[i, j] == 321)
                        {
                            builder.BuildShipHit();
                            enemyGameBoard[i, j].Source = builder.GetImage().Source;
                        }
                        else
                        {
                            if (enemyMatrix.playerMatrix[i, j] == 999)
                            {
                                builder.BuildHit();
                                enemyGameBoard[i, j].Source = builder.GetImage().Source;
                            }
                            else
                            {
                                if (enemyMatrix.playerMatrix[i, j] == 1123)
                                {
                                    builder.BuildMissPointer();
                                    enemyGameBoard[i, j].Source = builder.GetImage().Source;
                                }
                                else
                                {
                                    if (enemyMatrix.playerMatrix[i, j] == 1321)
                                    {
                                        builder.BuildShipPointer();
                                        enemyGameBoard[i, j].Source = builder.GetImage().Source;
                                    }
                                    else
                                    {
                                        if (enemyMatrix.playerMatrix[i, j] == 1999)
                                        {
                                            builder.BuildHitPointer();
                                            enemyGameBoard[i, j].Source = builder.GetImage().Source;
                                        }
                                        else
                                        {
                                            if (enemyMatrix.playerMatrix[i, j] == 1000)
                                            {
                                                builder.BuildWaterPointer();
                                                enemyGameBoard[i, j].Source = builder.GetImage().Source;
                                            }
                                            else
                                            {
                                                if (strtn == 0)
                                                {
                                                    builder.BuildWather();
                                                    enemyGameBoard[i, j] = builder.GetImage();
                                                }
                                                else
                                                {
                                                    builder.BuildWather();
                                                    enemyGameBoard[i, j].Source = builder.GetImage().Source;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    if (strtn == 0)
                    {
                        Grid.SetColumn(enemyGameBoard[i, j], j + 1);
                        Grid.SetRow(enemyGameBoard[i, j], i + 1);
                        Grid1.Children.Add(enemyGameBoard[i, j]);
                    }

                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {

                    if (playerMatrix.playerMatrix[i, j] == 0)
                    {
                        if (strtn == 0)
                        {
                            builder.BuildWather();
                            playerGameBoard[i, j] = builder.GetImage();
                        }
                        else
                        {
                            builder.BuildWather();
                            playerGameBoard[i, j].Source = builder.GetImage().Source;
                        }
                    }
                    else if (playerMatrix.playerMatrix[i, j] == 9)
                    {
                        if (strtn == 0)
                        {
                            builder.BuildWather();
                            playerGameBoard[i, j] = builder.GetImage();
                        }
                        else
                        {
                            builder.BuildWather();
                            playerGameBoard[i, j].Source = builder.GetImage().Source;
                        }
                    }
                    else if (playerMatrix.playerMatrix[i, j] == 1 || playerMatrix.playerMatrix[i, j] == 3 || playerMatrix.playerMatrix[i, j] == 5 || playerMatrix.playerMatrix[i, j] == 7)
                    {
                        if (strtn == 0)
                        {
                            builder.BuildShip();
                            playerGameBoard[i, j] = builder.GetImage();
                        }
                        else
                        {
                            builder.BuildShip();
                            playerGameBoard[i, j].Source = builder.GetImage().Source;
                        }
                    }
                    if (playerMatrix.playerMatrix[i, j] == 123)
                    {
                        builder.BuildMiss();
                        playerGameBoard[i, j].Source = builder.GetImage().Source;
                    }
                    if (playerMatrix.playerMatrix[i, j] == 321)
                    {
                        builder.BuildShipHit();
                        playerGameBoard[i, j].Source = builder.GetImage().Source;
                    }
                    if (playerMatrix.playerMatrix[i, j] == 999)
                    {
                        builder.BuildHit();
                        playerGameBoard[i, j].Source = builder.GetImage().Source;
                    }



                    if (strtn == 0)
                    {
                        Grid.SetColumn(playerGameBoard[i, j], i + 12);
                        Grid.SetRow(playerGameBoard[i, j], j + 1);
                        Grid1.Children.Add(playerGameBoard[i, j]);
                    }


                }
            }
            tb.Text = "";
            p2Name.Content = "Gracz";
            p1Name.Content = "Przeciwnik";
            if(strtn == 0)
            {
                enemyMatrix.playerMatrix[0, 0] += 1000;
                builder.BuildWaterPointer();
                enemyGameBoard[0, 0].Source = builder.GetImage().Source;
            }
            strtn = 1;
            // tu pierwszo ustawić celownik
           
        }

        private void RDY_Click(object sender, RoutedEventArgs e)
        {
            pointerLeft.IsEnabled = true;
            pointerRight.IsEnabled = true;
            pointerUp.IsEnabled = true;
            pointerDown.IsEnabled = true;
            ApprovedShot.IsEnabled = true;
            RDY.Visibility = Visibility.Hidden;
            proxy.Send("RDY");
            if(turns == 0 )
            {
                enemyTour();
            }
        }

        private void END_GAME(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}


