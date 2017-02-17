using Server.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClientWpf1.Content
{
    public class PointerMover
    {
        public Strategy strategy;

        public int [] Move(PlayerMatrix enemyMatrix, Image[,] enemyGameBoard, bool approved, int x, int y, ImageBuilder builder)
        {
            int[] tab;
           tab=  strategy.Move( enemyMatrix, enemyGameBoard,  approved,  x,  y,  builder);
            return tab;
        }
    }
}
