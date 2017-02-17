using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Classes;
using System.Windows.Controls;

namespace ClientWpf1.Content
{
    public abstract class Strategy
    {
        public abstract int[] Move(PlayerMatrix enemyMatrix, Image[,] enemyGameBoard,bool approved,int x,int y,ImageBuilder builder);
    }
}
