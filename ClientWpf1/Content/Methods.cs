using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientWpf1.Content
{
    public class Methods
    {
        public static Message ReadMessage(byte[] bb)
        {
            
            string flag = "";

            for (int i=0;i<4;i++)
            {
                flag+= Convert.ToChar(bb[i]);
            }

            string s = "";
            for (int i = 4; i < 7; i++)
                s += Convert.ToChar(bb[i]);

            int ii;
            Int32.TryParse(s, out ii);
            if (ii == 0)
            {
                return new Message(flag, null);
            }
            int[] tab;
            if (flag == "SUR1" || flag == "SUR2" ||  flag == "MIS1" || flag == "MIS2")
            {
                ii = 2 * ii;
                tab = new int[ii];
            }
            else
            {
                tab = new int[ii];
            }

            for (int i = 7; i <= ii + 6; i++)
            {
                s =""+ Convert.ToChar(bb[i]);
                int ielement;
                Int32.TryParse(s, out ielement);
                tab[i - 7] = ielement;
            }
            return new Message(flag, tab);

        }
    }
}
