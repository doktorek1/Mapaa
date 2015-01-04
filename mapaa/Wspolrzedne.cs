using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapaa
{
    class Wspolrzedne
    {
        private int a, b;

        public Wspolrzedne(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public int dajA() { return a; }
        public int dajB() { return b; }        
    }
}
