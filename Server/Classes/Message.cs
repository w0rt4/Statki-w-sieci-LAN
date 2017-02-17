using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Classes
{
    class Message : Iterator
    {
        public string flag { get; set; }
        public int[] tab { get; set; }
        byte[] b;
        int state = 0;
        int lenght;
        string temp="";

        public Message(byte[] b)
        {
            this.b = b;
            state = 0;
            lenght = b.Length;
        }

        public bool hasNext()
        {
            if (state == lenght)
                return false;
            return true;
        }

        public void getNext()
        {
            
         
            flag += Convert.ToChar(b[state]);
            state++;
            getNext(state);
            
        }
        
        private void getNext( int state)
        {
            this.state = state;
            if(hasNext())
            {
               if(state<4)
                {
                    flag += Convert.ToChar(b[state]);
                }
               else
                {
                    if(state<7)
                    {
                         temp += Convert.ToChar(b[state]);
                    }
                    else
                    {
                        if(state==7)
                        {
                            Int32.TryParse(temp, out lenght);
                            tab = new int[lenght];
                            lenght += 7;
                            temp= "" + Convert.ToChar(b[state]);
                            Int32.TryParse(temp, out tab[state-7]);

                        }
                        else
                        {
                            temp = "" + Convert.ToChar(b[state]);
                            Int32.TryParse(temp, out tab[state - 7]);
                        }
                    }
                }

                state++;
                getNext(state);
            }


        }

       
    }
}
