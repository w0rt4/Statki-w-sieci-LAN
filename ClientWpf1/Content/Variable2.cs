using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClientWpf1.Content
{
    class Variable2
    {
        
        public static int CZTEROMASZTOWCE = 0;
        public static int TRZYMASZTOWCE = 2;
        public static int DWUMASZTOWCE = 3;
        public static int JEDNOMASZTOWIEC = 4;

        public static ImageSource imgWater = new BitmapImage(new Uri("water.gif", UriKind.RelativeOrAbsolute));
        public static ImageSource imgHit = new BitmapImage(new Uri("statekZatopiony.gif", UriKind.RelativeOrAbsolute));
        public static ImageSource imgMiss = new BitmapImage(new Uri("miss.jpg", UriKind.RelativeOrAbsolute));
        public static ImageSource imgPointer = new BitmapImage(new Uri("wodaCelownik.gif", UriKind.RelativeOrAbsolute));
        public static ImageSource imgShip = new BitmapImage(new Uri("statekTrafiony.gif", UriKind.RelativeOrAbsolute));
        public static ImageSource imgPlaceShip = new BitmapImage(new Uri("statek.gif", UriKind.RelativeOrAbsolute));
        public static ImageSource imgPointerMiss = new BitmapImage(new Uri("wodaCelownikpudlo.jpg", UriKind.RelativeOrAbsolute));
        public static ImageSource imgPointerShip = new BitmapImage(new Uri("trafionyCelownik.gif", UriKind.RelativeOrAbsolute));
        public static ImageSource imgPointerHit = new BitmapImage(new Uri("zatopionyCelownik.gif", UriKind.RelativeOrAbsolute));
        public static ImageSource imgShipColide = new BitmapImage(new Uri("statekKolizja.gif", UriKind.RelativeOrAbsolute));
        public static ImageSource imgLogo = new BitmapImage(new Uri("battleship.jpg", UriKind.RelativeOrAbsolute));



        public Variable2()
        {
            CZTEROMASZTOWCE = 0;
            TRZYMASZTOWCE = 2;
            DWUMASZTOWCE = 3;
            JEDNOMASZTOWIEC = 4;
        }
    }
}
