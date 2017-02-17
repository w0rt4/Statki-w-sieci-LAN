using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Classes
{
    public class PlayerMatrix
    {
        public int[,] playerMatrix { get; set; }

        public PlayerMatrix()
        {
            playerMatrix = new int[10,10];
        }

    }
}
