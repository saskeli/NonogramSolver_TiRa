using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;

namespace SolverConsole
{
    class Program
    {
        static private readonly string simple =
            "width 4\r\nheight 5\r\n\r\nrows\r\n4\r\n1,1\r\n1,1\r\n1,2\r\n3\r\n\r\ncolumns\r\n5\r\n1,1\r\n1,2\r\n4";

        static void Main(string[] args)
        {
            Nonogram ng = new Nonogram(simple);
        }
    }
}
