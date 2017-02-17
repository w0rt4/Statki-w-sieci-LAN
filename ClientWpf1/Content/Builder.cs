using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientWpf1.Content
{
    public interface Builder
    {
        void BuildWather();
        void BuildHit();
        void BuildMiss();
        void BuildColide();
        void BuildShip();
        void BuildWaterPointer();
        void BuildShipPointer();
        void BuildMissPointer();
        void BuildHitPointer();
        void BuildShipHit();
        Image GetImage();
        

    }
}
