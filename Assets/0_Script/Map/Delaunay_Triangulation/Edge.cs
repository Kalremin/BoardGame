using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Edge
{
    int a, b;
    //Edge() { a = b = 0; }
    Edge(int a, int b)
    {
        if (a > b)
        {
            this.a = a;
            this.b = b;
        }
        else
        {
            this.a = b;
            this.b = a;
        }
    }

    bool Equal(Edge x) {return a==x.a && b==x.b;}
    
    bool Compare(Edge x)
    {
        if (this.a == x.a) return this.b < x.b;
        return this.a < x.a;
    }

}
