using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            TimeSpan ts;
            foreach (string file in Directory.GetFiles(Regex.Replace(Environment.CurrentDirectory, "NonogramSolver.*", @"NonogramSolver\\Data")))
            {
                Console.WriteLine("Benchmarking file: " + Path.GetFileName(file));
                Nonogram ng = NonoGramFactory.ParseFromFile(file);
                ISolver s = new SerialSolver(true);
                s.Run(ng);
                Console.WriteLine("Solvable with SerialSolver: " + s.Solved());
                if (s.Solved())
                {
                    ts = TimeSpan.Zero;
                    for (int i = 0; i < 1000; i++)
                    {
                        s.Run(ng);
                        ts = ts.Add(s.BenchTime());
                    }
                    Console.WriteLine("Average solving time: " + (ts.TotalMilliseconds / 1000) + "ms");
                }
            }

            Console.WriteLine("Any key to terminate.");
            Console.ReadKey();
        }
    }
}
