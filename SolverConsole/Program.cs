using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;

namespace SolverConsole
{
    internal class Program
    {
        private const string Simple = "4,5\r\n4\r\n1,1\r\n1,1\r\n1,2\r\n3\r\n5\r\n1,1\r\n1,2\r\n4\r\n";

        private static void Main(string[] args)
        {
            Nonogram ng = NonoGramFactory.ParseFromString(Simple);
        }
    }
}
