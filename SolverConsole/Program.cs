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
            // Nonogram ng = NonoGramFactory.ParseFromString(Simple);
            Nonogram ng = NonoGramFactory.ParseFromFile(Path.Combine(Environment.CurrentDirectory, "Data/joker.txt"));
            sw.Stop();
            Console.WriteLine("Nonogram parsed in " + sw.Elapsed.TotalMilliseconds + "ms.");
            ISolver sol = new InitSolver();
            sol.Run(ng);
            Console.WriteLine("Solved: " + sol.Solved());
            Console.WriteLine("Runtime: " + sol.BenchTime().TotalMilliseconds + "ms.");
            while (!sol.Results().IsEmpty)
            {
                Result res = sol.Results().Dequeue();
                ng.Set(res.Row, res.Column, res.State);
            }
            Console.WriteLine(ng);

            Console.WriteLine("Any key to terminate.");
            Console.ReadKey();
        }
    }
}
