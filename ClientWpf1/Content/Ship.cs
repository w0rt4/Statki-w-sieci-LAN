using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientWpf1.Content
{
    public class Ship
    {
        public int x { get; set; }
        public int y { get; set; }
        public bool rotate { get; set; }
        public int size { get; set; }


        public Ship(int size)
        {
            rotate = false;
            x = 0;
            y = 0;
            this.size = size;
        }
    }
}
