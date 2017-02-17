using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Server.Classes;

namespace ClientWpf1.Content
{
    public class MoveLeft : Strategy
    {

        public override int[] Move(PlayerMatrix enemyMatrix, Image[,] enemyGameBoard, bool approved, int x, int y, ImageBuilder builder)
        {
            int[] tab = new int[2];
            if (y > 0)
            {

                enemyMatrix.playerMatrix[x, y] -= 1000;
                y--;
                enemyMatrix.playerMatrix[x, y] += 1000;
                if (enemyMatrix.playerMatrix[x, y] == 1000 || enemyMatrix.playerMatrix[x, y] == 1009 || enemyMatrix.playerMatrix[x, y] == 1001 || enemyMatrix.playerMatrix[x, y] == 1003 || enemyMatrix.playerMatrix[x, y] == 1005 || enemyMatrix.playerMatrix[x, y] == 1007)
                {
                    builder.BuildWaterPointer();
                    enemyGameBoard[x, y].Source = builder.GetImage().Source;
                }
                if (enemyMatrix.playerMatrix[x, y] == 1123)
                {
                    builder.BuildMissPointer();
                    enemyGameBoard[x, y].Source = builder.GetImage().Source;
                }
                if (enemyMatrix.playerMatrix[x, y] == 1321)
                {
                    builder.BuildShipPointer();
                    enemyGameBoard[x, y].Source = builder.GetImage().Source;
                }
                if (enemyMatrix.playerMatrix[x, y] == 1999)
                {
                    builder.BuildHitPointer();
                    enemyGameBoard[x, y].Source = builder.GetImage().Source;
                }
                if (enemyMatrix.playerMatrix[x, y + 1] == 123)
                {
                    builder.BuildMiss();
                    enemyGameBoard[x, y + 1].Source = builder.GetImage().Source;
                }
                if (enemyMatrix.playerMatrix[x, y + 1] == 321)
                {
                    builder.BuildShipHit();
                    enemyGameBoard[x, y + 1].Source = builder.GetImage().Source;
                }
                if (enemyMatrix.playerMatrix[x, y + 1] == 999)
                {
                    builder.BuildHit();
                    enemyGameBoard[x, y + 1].Source = builder.GetImage().Source;
                }
                if (enemyMatrix.playerMatrix[x, y + 1] == 0 || enemyMatrix.playerMatrix[x, y + 1] == 9 || enemyMatrix.playerMatrix[x, y + 1] == 1 || enemyMatrix.playerMatrix[x, y + 1] == 3 || enemyMatrix.playerMatrix[x, y + 1] == 5 || enemyMatrix.playerMatrix[x, y + 1] == 7)
                {
                    builder.BuildWather();
                    enemyGameBoard[x, y + 1].Source = builder.GetImage().Source;
                }

                if (enemyMatrix.playerMatrix[x, y] == 1000 || enemyMatrix.playerMatrix[x, y] == 1009 || enemyMatrix.playerMatrix[x, y] == 1001 || enemyMatrix.playerMatrix[x, y] == 1003 || enemyMatrix.playerMatrix[x, y] == 1005 || enemyMatrix.playerMatrix[x, y] == 1007)
                {
                    tab[0] = y;
                    tab[1] = 1;
                    return tab;
                }
                else
                {
                    tab[0] = y;
                    tab[1] = 0;
                    return tab;
                }
            }
            if (enemyMatrix.playerMatrix[x, y] == 1000 || enemyMatrix.playerMatrix[x, y] == 1009 || enemyMatrix.playerMatrix[x, y] == 1001 || enemyMatrix.playerMatrix[x, y] == 1003 || enemyMatrix.playerMatrix[x, y] == 1005 || enemyMatrix.playerMatrix[x, y] == 1007)
            {
                tab[0] = y;
                tab[1] = 1;
                return tab;
            }
            else
            {
                tab[0] = y;
                tab[1] = 0;
                return tab;
            }

        }

    }
}
