using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClientWpf1.Content
{
    public class ImageBuilder : Builder
    {
        private Image image;
        public void BuildColide()
        {
            image = new Image
            {
                Source = Variable2.imgShipColide,      //kolizja
                Stretch = Stretch.Fill
            };
        }

        public void BuildHit()
        {
            image = new Image
            {
                Source = Variable2.imgHit,
                Stretch = Stretch.Fill
            };
        }

        public void BuildShipHit()
        {
            image = new Image
            {
                Source = Variable2.imgShip,
                Stretch = Stretch.Fill
            };
        }

        public void BuildHitPointer()
        {
            image = new Image
            {
                Source = Variable2.imgPointerHit,
                Stretch = Stretch.Fill
            };
        }

        public void BuildMiss()
        {
           image = new Image
           {
               Source = Variable2.imgMiss,  //jeżeli to 9 to tu nie można stawiać 
               Stretch = Stretch.Fill
           };
        }

        public void BuildMissPointer()
        {
            image = new Image
            {
                Source = Variable2.imgPointerMiss,
                Stretch = Stretch.Fill
            };
        }

        public void BuildShip()
        {
            image = new Image
            {
                Source = Variable2.imgPlaceShip,        // to statek
                Stretch = Stretch.Fill
            };
        }

        public void BuildShipPointer()
        {
            image = new Image
            {
                Source = Variable2.imgPointerShip,
                Stretch = Stretch.Fill
            };
        }

        public void BuildWaterPointer()
        {
            image = new Image
            {
                Source = Variable2.imgPointer,
                Stretch = Stretch.Fill
            };
        }

        public void BuildWather()
        {
            image = new Image
            {
                Source = Variable2.imgWater,  // zero znaczy nic tu nie ma woda
                Stretch = Stretch.Fill
            };
        }   

        public Image GetImage()
        {
            return image;
        }
    }
}
