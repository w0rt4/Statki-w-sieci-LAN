using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientWpf1.Content
{
    public class Message
    {
        public string flag { get; set; }
        public int [] tab { get; set; }

        public Message(string f, int [] t)
        {
            flag = f;
            tab = t;
        }
    }
}
