using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;
using SolverLib;

namespace SolverConsole
{
    internal class Program
    {
        private const string Simple = "4,5\r\n4\r\n1,1\r\n1,1\r\n1,2\r\n3\r\n5\r\n1,1\r\n1,2\r\n4\r\n";

        private static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Nonogram ng = NonoGramFactory.ParseFromString(Simple);
            sw.Stop();
            Console.WriteLine("Nonogram parsed in " + sw.Elapsed.TotalMilliseconds + "ms.");
            TreeSolver ts = new TreeSolver();
            ts.Run(ng);
            Console.WriteLine("Solved with TreeSolver: " + ts.Solved());
            Console.WriteLine("TreeSolver runtime: " + ts.BenchTime().TotalMilliseconds + "ms");

            sw.Restart();
            ng = NonoGramFactory.ParseFromFile(Path.Combine(Environment.CurrentDirectory, "Data/joker.txt"));
            sw.Stop();
            Console.WriteLine("Nonogram parsed in " + sw.Elapsed.TotalMilliseconds + "ms.");
            ts.Run(ng);
            Console.WriteLine("Solved with TreeSolver: " + ts.Solved());
            Console.WriteLine("TreeSolver runtime: " + ts.BenchTime().TotalMilliseconds + "ms");
            Console.WriteLine(ng);
            Console.WriteLine("Any key to terminate.");
            Console.ReadKey();
        }
    }
}
